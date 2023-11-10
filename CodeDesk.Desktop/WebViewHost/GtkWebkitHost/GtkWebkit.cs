using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using System.Text;
using CodeDesk.Desktop.NativeHost.LinuxHost;
using Newtonsoft.Json;

namespace CodeDesk.Desktop.WebViewHost.GtkWebkitHost
{
    internal class GtkWebkitApi
    {
        private const string Libraries = "libwebkit2gtk-4.0.so.37";

        [DllImport(Libraries)]
        internal static extern IntPtr webkit_web_view_new();
        [DllImport(Libraries)]
        internal static extern void webkit_web_view_load_uri(IntPtr webView, string uri);
        [DllImport(Libraries)]
        internal static extern uint webkit_get_major_version();
        [DllImport(Libraries)]
        internal static extern uint webkit_get_minor_version();
        [DllImport(Libraries)]
        internal static extern uint webkit_get_micro_version();
        [DllImport(Libraries)]
        internal static extern IntPtr webkit_user_script_new(string source, int injectionTime, int injectionFlags, IntPtr whitelist, IntPtr blacklist);
        [DllImport(Libraries)]
        internal static extern IntPtr webkit_user_content_manager_new();
        [DllImport(Libraries)]
        internal static extern IntPtr webkit_web_view_new_with_user_content_manager(IntPtr manager);
        [DllImport(Libraries)]
        internal static extern void webkit_user_content_manager_add_script(IntPtr manager, IntPtr script);
        [DllImport(Libraries)]
        internal static extern void webkit_user_script_unref(IntPtr script);
        [DllImport(Libraries)]
        internal static extern void webkit_settings_set_enable_developer_extras(IntPtr settings, bool enable);
        [DllImport(Libraries)]
        internal static extern IntPtr webkit_web_view_get_settings(IntPtr webView);
        [DllImport(Libraries)]
        internal static extern IntPtr webkit_web_context_get_default();
        [DllImport(Libraries)]
        internal static extern void webkit_web_context_register_uri_scheme(IntPtr context, string scheme, IntPtr callback, IntPtr user_data, IntPtr destroy_notify);
        [DllImport(Libraries)]
        internal static extern IntPtr webkit_uri_scheme_request_get_uri(IntPtr request);
        [DllImport(Libraries)]
        internal static extern void webkit_uri_scheme_request_finish(IntPtr request, IntPtr stream, int length, IntPtr mimeType);
        [DllImport(Libraries)]
        internal static extern void webkit_user_content_manager_register_script_message_handler(IntPtr manager, string name);
        [DllImport(Libraries)]
        internal static extern IntPtr webkit_javascript_result_get_js_value(IntPtr result);
        [DllImport(Libraries)]
        internal static extern IntPtr jsc_value_to_string(IntPtr value);
        [DllImport(Libraries)]
        internal static extern bool jsc_value_is_string(IntPtr value);
        [DllImport(Libraries)]
        internal static extern IntPtr webkit_web_view_run_javascript(IntPtr webView, string script, IntPtr cancellable, IntPtr callback, IntPtr userData);
        [DllImport(Libraries)]
        internal static extern void webkit_context_menu_remove_all(IntPtr menu);
    }

    internal class GtkWebkit : IWebView
    {
        public string Version =>
            $"GtkWebkit {GtkWebkitApi.webkit_get_major_version()}.{GtkWebkitApi.webkit_get_minor_version()}.{GtkWebkitApi.webkit_get_micro_version()}";

        public IntPtr Handle { get; set; }
        public async Task<(bool Succeed, string Message)> CheckWebView(Action<double> progress)
        {
            return (true, "");
        }
        // 定义回调函数的委托类型
        private delegate void ContextMenuCallbackDelegate(IntPtr webView, IntPtr menu, IntPtr userData);

        // 定义回调函数
        private static void ContextMenuCallback(IntPtr webView, IntPtr menu, IntPtr userData)
        {
            GtkWebkitApi.webkit_context_menu_remove_all(menu);
        }

        private BlazorWebViewManager webViewManager;
        public async Task Initialization(IWindow window, int width, int height, string url, Type blazorComponent, string blazorSelector)
        {
            var contentManager = GtkWebkitApi.webkit_user_content_manager_new();
            Handle = GtkWebkitApi.webkit_web_view_new_with_user_content_manager(contentManager);
            GtkApi.gtk_container_add(window.Handle, Handle);

            //禁止新窗口打开
            // CoreWebView2Controller.CoreWebView2.NewWindowRequested += (s, e) =>
            // {
            //     if (e.NewWindow == null)
            //         e.NewWindow = (CoreWebView2)s;
            // };
            if (IsDebug == true)
            {
                IntPtr settings = GtkWebkitApi.webkit_web_view_get_settings(Handle);
                GtkWebkitApi.webkit_settings_set_enable_developer_extras(settings, true);
            }
            else
            {
                IntPtr settings = GtkWebkitApi.webkit_web_view_get_settings(Handle);
                GtkWebkitApi.webkit_settings_set_enable_developer_extras(settings, false);
                GtkApi.g_signal_connect_data(Handle, "context-menu", Marshal.GetFunctionPointerForDelegate(new ContextMenuCallbackDelegate(ContextMenuCallback)), IntPtr.Zero, IntPtr.Zero, 0);
            }

            if (WebAppType != WebAppType.Http)
            {
                var schemeConfig = new Uri(url).ParseScheme();
                var dispatcher = new BlazorDispatcher(window);
                webViewManager = new BlazorWebViewManager(window, this, Application.Services.Value, dispatcher,
                    Application.FileProvider, Application.Services.Value.GetService<JSComponentConfigurationStore>(),
                    schemeConfig);
                if (WebAppType == WebAppType.Blazor)
                {
                    _ = dispatcher.InvokeAsync(async () =>
                    {
                        await webViewManager.AddRootComponentAsync(blazorComponent, blazorSelector, ParameterView.Empty);
                    });
                }

                UriSchemeCallbackFunc uriSchemeCallback = UriSchemeCallback;
                var context = GtkWebkitApi.webkit_web_context_get_default();
                GtkWebkitApi.webkit_web_context_register_uri_scheme(context, schemeConfig.AppScheme,Marshal.GetFunctionPointerForDelegate(uriSchemeCallback),IntPtr.Zero, IntPtr.Zero);

                //injectionTime = 0; // WEBKIT_USER_SCRIPT_INJECT_AT_DOCUMENT_START,injectionFlags = 2; // WEBKIT_USER_CONTENT_INJECT_ALL_FRAMES
                var script = GtkWebkitApi.webkit_user_script_new(
                    Application.FileProvider.GetEmbeddedManifestContent("edge.document.webkit.js"),
                    2, 0, IntPtr.Zero, IntPtr.Zero);
                GtkWebkitApi.webkit_user_content_manager_add_script(contentManager, script);
                GtkWebkitApi.webkit_user_script_unref(script);

                GtkApi.g_signal_connect_data(contentManager,
                    "script-message-received::CodeDeskInterop",
                    Marshal.GetFunctionPointerForDelegate(new ScriptMessageReceivedDelegate(ScriptMessageReceived)),
                    IntPtr.Zero, IntPtr.Zero, 0);
                GtkWebkitApi.webkit_user_content_manager_register_script_message_handler(contentManager,"CodeDeskInterop");
                webViewManager.Navigate("/");


                IntPtr UriSchemeCallback(IntPtr request, IntPtr user_data)
                {
                    var uri = Marshal.PtrToStringAnsi(GtkWebkitApi.webkit_uri_scheme_request_get_uri(request));
                    var response = webViewManager.OnResourceRequested(schemeConfig, uri);
                    var responsePtr = response.Content.StreamToIntptr();
                    var stream = GtkApi.g_memory_input_stream_new_from_data(responsePtr.IntPtr, responsePtr.Length, IntPtr.Zero);
                    GtkWebkitApi.webkit_uri_scheme_request_finish(request, stream, responsePtr.Length,Marshal.StringToCoTaskMemAnsi(response.Type));

                    return IntPtr.Zero;
                }


                void ScriptMessageReceived(IntPtr webView, IntPtr message, IntPtr userData)
                {
                    IntPtr jsResult = GtkWebkitApi.webkit_javascript_result_get_js_value(message);
                    if (GtkWebkitApi.jsc_value_is_string(jsResult))
                    {
                        IntPtr jsString = GtkWebkitApi.jsc_value_to_string(jsResult);
                        string messageString = Marshal.PtrToStringAnsi(jsString);
                        webViewManager.OnMessageReceived(schemeConfig.AppOrigin, messageString);
                    }
                }
            }
            else
            {
                GtkWebkitApi.webkit_web_view_load_uri(Handle, url);
            }

        }

        private delegate void ScriptMessageReceivedDelegate(IntPtr webView, IntPtr message, IntPtr userData);

        private delegate IntPtr UriSchemeCallbackFunc(IntPtr request, IntPtr user_data);

        public void SizeChange(IntPtr handle, int width, int height)
        {
            // if (CoreWebView2Controller != null)
            //     CoreWebView2Controller.Bounds = new Rectangle(0, 0, width, height);
        }

        public void Navigate(string url)
        {
            GtkWebkitApi.webkit_web_view_load_uri(Handle, url);
        }
        public void SendWebMessage(string message)
        {
            var js = new StringBuilder();
            js.Append("__dispatchMessageCallback(\"");
            //js.Append(JsonConvert.ToString(message));
            
            js.Append(message.EscapeJson());
            js.Append("\")");
            GtkWebkitApi.webkit_web_view_run_javascript(Handle,
                js.ToString(), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }
        public void SendWebMessage(SystemMessageModel message)
        {
            SendWebMessage(message.ToJson());
        }

        private bool IsDebug = false;

        public void SetDebug(bool debug)
        {
            this.IsDebug = debug;
        }

        private WebAppType WebAppType;

        public void SetWebAppType(WebAppType type)
        {
            this.WebAppType = type;
        }
    }
}