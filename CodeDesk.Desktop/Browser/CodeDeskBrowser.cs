using Xilium.CefGlue;

namespace CodeDesk.Desktop.Browser
{
    internal class CodeDeskBrowser
    {
        IWindow Window { get; set; }
        internal CodeDeskBrowser CreateBrowser(IWindow window)
        {
            Window = window;
            var windowInfo = CefWindowInfo.Create();
            windowInfo.SetAsChild(window.Handle, new CefRectangle { X = 0, Y = 0, Width = window.Options.Size.Width, Height = window.Options.Size.Height });

            CefBrowserHost.CreateBrowser(windowInfo, new CodeDeskBrowserClient(this), new CefBrowserSettings(), window.Options.StartUrl);
            return this;
        }
        internal CefBrowser browser;
        internal void OnCreated(CefBrowser browser)
        {
            this.browser = browser;
            Window.SetBrowser(browser);


            browser.GetMainFrame().LoadUrl("/");
            var eee = AppBuilder.FileProvider.GetEmbeddedManifestContent("blazor.document.js");

            //AppBuilder.webViewManager.Navigate("/");
        }
    }
}
