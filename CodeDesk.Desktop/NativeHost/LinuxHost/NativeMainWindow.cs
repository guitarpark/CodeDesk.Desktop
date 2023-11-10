using System.Diagnostics;
using System.Runtime.InteropServices;
using CodeDesk.Desktop.WebViewHost.GtkWebkitHost;

namespace CodeDesk.Desktop.NativeHost.LinuxHost
{
    internal class NativeMainWindow : NativeWindowBase, IWindow
    {
        public Type BlazorComponent { get; set; }
        public string BlazorSelector { get; set; }
        private IWebView webView;

        string Url;
        
        public NativeMainWindow(WindowConfig option)
        {
            base.MaximumSize = option.MaximumSize;
            base.MinimumSize = option.MinimumSize;
            base.Size = option.Size;
            base.Chromeless = option.Chromeless;
            base.CanMinMax = option.CanMinMax;
            base.CanReSize = option.CanReSize;
            base.StartupCenter = option.StartupCenter;
            base.WindowState = option.WindowState;
            this.Url = option.Url;
            this.isDebug = option.IsDebug;
            this.webAppType = option.WebAppType;

            BlazorComponent = option.BlazorComponent;
            BlazorSelector = option.BlazorSelector;

            base.OnSizeChange += (s, e) =>
            {
                //  webView.SizeChange(Handle, e.Width, e.Height);
            };
        }

        private bool isDebug;
        private WebAppType webAppType;
        public override void Show()
        {
            webView = new GtkWebkit();
            webView.SetDebug(isDebug);
            webView.SetWebAppType(webAppType);
            webView.Initialization(this, 300, 400, this.Url, BlazorComponent, BlazorSelector);
            
            RegisterHandel(webView.Handle,"button-press-event", ((widget, ev, data) => base.OnButtonPressEvent(base.Handle, ev, data)));
            RegisterHandel(webView.Handle,"button-release-event",((widget, ev, data) => base.OnButtonReleaseEvent(base.Handle, ev, data)));
            
            base.Show();
        }
        public override void Close()
        {
            base.ExitApplication();
        }
        public void OpenSystemBroswer(string url)
        {
            var process = new Process();
            process.StartInfo.FileName = "xdg-open";
            process.StartInfo.Arguments = url;
            process.Start();
        }
    }
}