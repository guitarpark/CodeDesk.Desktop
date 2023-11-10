using CodeDesk.Desktop.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Drawing;
using System.Reflection.Metadata;

namespace CodeDesk.Desktop.WebViewHost.WebView2Host
{
    internal class WebView2 : IWebView
    {
        public IntPtr Handle { get; set; }
        public string Version => CoreWebView2Environment.GetAvailableBrowserVersionString();

        public async Task<(bool Succeed, string Message)> CheckWebView(Action<double> progress)
        {
            var saveFile = UtilitiesExtensions.GetApplicationPath($"WebView2-{Guid.NewGuid()}.exe");
            try
            {
                if (await CoreWebView2Environment.CreateAsync() == null)
                {
                    throw new Exception("运行库缺失");
                }

                return (true, "");
            }
            catch (Exception ex)
            {
                try
                {
                    var url =
                        "https://msedge.sf.dl.delivery.mp.microsoft.com/filestreamingservice/files/03e33965-9ac7-458c-b6bc-c48177736a31/MicrosoftEdgeWebview2Setup.exe";
                    if (Application.Is64Bit)
                    {
                        url =
                            "https://msedge.sf.dl.delivery.mp.microsoft.com/filestreamingservice/files/d4e1ef21-802b-4d11-9d18-910a4fa6c127/MicrosoftEdgeWebView2RuntimeInstallerX64.exe";
                    }

                    await HttpExtensions.DownloadFile(url, saveFile, progress);

                    await Task.Run(() =>
                    {
                        UtilitiesExtensions.ExecuteCommand("saveFile", "/silent /install", null);
                    });
                }
                catch (Exception e)
                {
                    return (false, e.Message);
                    ;
                }
                finally
                {
                    System.IO.File.Delete(saveFile);
                }
            }

            return (true, "");
            ;
        }

        private CoreWebView2Controller CoreWebView2Controller { get; set; }
        private CoreWebView2Environment environment { get; set; }

        public async Task Initialization(IWindow window, int width, int height, string url, Type blazorComponent,
            string blazorSelector)
        {
            var userPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Process.GetCurrentProcess().ProcessName);
            environment = await CoreWebView2Environment.CreateAsync(userDataFolder: userPath);
            CoreWebView2Controller = await environment.CreateCoreWebView2ControllerAsync(window.Handle);
            
            CoreWebView2Controller.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            CoreWebView2Controller.CoreWebView2.Settings.IsStatusBarEnabled = false;
            CoreWebView2Controller.CoreWebView2.Settings.IsZoomControlEnabled = false;
            CoreWebView2Controller.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
            CoreWebView2Controller.Bounds = new Rectangle(0, 0, width, height);

            //禁止新窗口打开
            CoreWebView2Controller.CoreWebView2.NewWindowRequested += (s, e) =>
            {
                if (e.NewWindow == null)
                    e.NewWindow = (CoreWebView2)s;
            };
            //屏蔽快捷键
            CoreWebView2Controller.AcceleratorKeyPressed += (s, e) =>
            {
                if (IsDebug == false && e.VirtualKey >= 112 && e.VirtualKey <= 122)
                    e.Handled = true;
            };

            if (IsDebug == true)
            {
                CoreWebView2Controller.CoreWebView2.Settings.AreDevToolsEnabled = true;
                CoreWebView2Controller.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
            }
            else
            {
                CoreWebView2Controller.CoreWebView2.Settings.AreDevToolsEnabled = false;
                CoreWebView2Controller.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            }

            if (WebAppType != WebAppType.Http)
            {
                var schemeConfig = new Uri(url).ParseScheme();
                var dispatcher = new BlazorDispatcher(window);
                var webViewManager = new BlazorWebViewManager(window, this, Application.Services.Value, dispatcher,
                    Application.FileProvider, Application.Services.Value.GetService<JSComponentConfigurationStore>(),
                    schemeConfig);
                if (WebAppType == WebAppType.Blazor)
                {
                    _ = dispatcher.InvokeAsync(async () =>
                    {
                        await webViewManager.AddRootComponentAsync(blazorComponent, blazorSelector,
                            ParameterView.Empty);
                    });
                }

                CoreWebView2Controller.CoreWebView2.WebMessageReceived += (s, e) =>
                {
                    webViewManager.OnMessageReceived(e.Source, e.TryGetWebMessageAsString());
                };
                CoreWebView2Controller.CoreWebView2.AddWebResourceRequestedFilter($"{schemeConfig.AppOrigin}*",
                    CoreWebView2WebResourceContext.All);
                CoreWebView2Controller.CoreWebView2.WebResourceRequested += (s, e) =>
                {
                    var response = webViewManager.OnResourceRequested(schemeConfig, e.Request.Uri.ToString());
                    if (response.Content != null)
                    {
                        e.Response = environment.CreateWebResourceResponse(response.Content, 200, "OK",
                            $"Content-Type:{response.Type}");
                    }
                };
                await CoreWebView2Controller.CoreWebView2
                    .AddScriptToExecuteOnDocumentCreatedAsync(
                        Application.FileProvider.GetEmbeddedManifestContent("edge.document.js")).ConfigureAwait(true);


                webViewManager.Navigate("/");
            }
            else
            {
                CoreWebView2Controller.CoreWebView2.Navigate(url);
            }
        }

        public void SizeChange(IntPtr handle, int width, int height)
        {
            if (CoreWebView2Controller != null)
                CoreWebView2Controller.Bounds = new Rectangle(0, 0, width, height);
        }

        public void Navigate(string url)
        {
            CoreWebView2Controller?.CoreWebView2.Navigate(url);
        }

        public void SendWebMessage(string message)
        {
            CoreWebView2Controller.CoreWebView2.PostWebMessageAsString(message);
        }

        public void SendWebMessage(SystemMessageModel message)
        {
            CoreWebView2Controller.CoreWebView2.PostWebMessageAsString(message.ToJson());
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