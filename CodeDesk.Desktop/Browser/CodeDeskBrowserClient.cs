using CodeDesk.Desktop.Browser;
using CodeDesk.Desktop.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using Xilium.CefGlue;
using static Xilium.CefGlue.BrowserRequestHandler;
using static Xilium.CefGlue.Wrapper.CefMessageRouterBrowserSide;

namespace CodeDesk.Desktop.Browser
{
    internal class CodeDeskBrowserClient : CefClient
    {
        private CefLifeSpanHandler lifeSpanHandler;
        private BrowserContextMenuHandler contextMenuHandler;
        private BrowserRequestHandler requestHandler;
        public CodeDeskBrowserClient(CodeDeskBrowser browser)
        {
            lifeSpanHandler = new BrowserLifeSpanHandler(browser);
            contextMenuHandler = new BrowserContextMenuHandler();
            requestHandler = new BrowserRequestHandler();
        }
        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return lifeSpanHandler;
        }
        protected override CefContextMenuHandler GetContextMenuHandler()
        {
            return contextMenuHandler;
        }
        protected override CefRequestHandler GetRequestHandler()
        {
            return requestHandler;
        }
        protected override bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
        {
            //AppBuilder.webViewManager.OnMessageReceived(frame.Url.ToString(),message.)
            return base.OnProcessMessageReceived(browser, frame, sourceProcess, message);
        }
    }
}
namespace Xilium.CefGlue
{
    internal class BrowserLifeSpanHandler : CefLifeSpanHandler
    {
        CodeDeskBrowser core;
        internal BrowserLifeSpanHandler(CodeDeskBrowser browser)
        {
            this.core = browser;
        }
        protected override void OnAfterCreated(CefBrowser browser)
        {
            base.OnAfterCreated(browser);

            core.OnCreated(browser);
        }
        protected override bool OnBeforePopup(CefBrowser browser, CefFrame frame, string targetUrl, string targetFrameName, CefWindowOpenDisposition targetDisposition, bool userGesture, CefPopupFeatures popupFeatures, CefWindowInfo windowInfo, ref CefClient client, CefBrowserSettings settings, ref CefDictionaryValue extraInfo, ref bool noJavascriptAccess)
        {
            browser.GetMainFrame().LoadUrl(targetUrl);
            return true;


            return base.OnBeforePopup(browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, popupFeatures, windowInfo, ref client, settings, ref extraInfo, ref noJavascriptAccess);
        }
    }

    internal sealed class BrowserContextMenuHandler : CefContextMenuHandler
    {
        protected override void OnBeforeContextMenu(CefBrowser browser, CefFrame frame, CefContextMenuParams state, CefMenuModel model)
        {
            model.AddItem((int)CefMenuId.Copy, "刷新");
            model.AddItem(10, "开发者模式");

            base.OnBeforeContextMenu(browser, frame, state, model);
        }
        protected override bool OnContextMenuCommand(CefBrowser browser, CefFrame frame, CefContextMenuParams state, int commandId, CefEventFlags eventFlags)
        {
            if (commandId == (int)CefMenuId.Copy)
            {
                browser.Reload();
                return true;
            }
            if (commandId == 10)
            {
                var wi = CefWindowInfo.Create();
                wi.SetAsPopup(IntPtr.Zero, "DevTools");
                browser.GetHost().ShowDevTools(wi, new DevToolsWebClient(), new CefBrowserSettings(), new CefPoint(0, 0));
                return true;
            }
            return base.OnContextMenuCommand(browser, frame, state, commandId, eventFlags);
        }


        private class DevToolsWebClient : CefClient
        {
        }
    }

    internal class BrowserRequestHandler : CefRequestHandler
    {
        protected override CefResourceRequestHandler GetResourceRequestHandler(CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            //判断网址如果满足，那么走自定义
            return new BrowserResourceRequestHandler();
        }

        internal class BrowserResourceRequestHandler : CefResourceRequestHandler
        {
            protected override CefCookieAccessFilter GetCookieAccessFilter(CefBrowser browser, CefFrame frame, CefRequest request)
            {
                return null;
            }
            protected override CefResourceHandler GetResourceHandler(CefBrowser browser, CefFrame frame, CefRequest request)
            {
                return new BrowserResourceHandler(request.Url);
            }
        }
        internal class BrowserResourceHandler : CefResourceHandler
        {
            private byte[] resourceData;
            string mimeType;
            internal BrowserResourceHandler(string url)
            {
                var info = ResponseContenExtensions.ResourceRequested(url);
                mimeType = info.MimeType;
                using (BinaryReader binaryReader = new BinaryReader(info.Content))
                {
                    long length = info.Content.Length;
                    this.resourceData = new byte[length];
                    binaryReader.Read(this.resourceData, 0, this.resourceData.Length);
                }
            }
            protected override void Cancel()
            {
            }

            protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
            {
                response.MimeType = mimeType;
                response.Status = 200;
                responseLength = this.resourceData.Length;
                redirectUrl = null;

            }

            protected override bool Open(CefRequest request, out bool handleRequest, CefCallback callback)
            {
                handleRequest = true;
                return true;
            }

            private int dataReadCount = 0;
            protected unsafe override bool Read(IntPtr dataOut, int bytesToRead, out int bytesRead, CefResourceReadCallback callback)
            {

                int leftToRead = this.resourceData.Length - this.dataReadCount;
                if (leftToRead == 0)
                {
                    bytesRead = 0;
                    return false;
                }
                int outCount = dataOut.GetIntPtrLength();
                //开始读取
                bytesRead = Math.Min(outCount, leftToRead);
                Marshal.Copy(resourceData,dataReadCount,dataOut, bytesRead);
                
                dataReadCount+= bytesRead;
                return true;
            }

            protected override bool Skip(long bytesToSkip, out long bytesSkipped, CefResourceSkipCallback callback)
            {
                bytesSkipped = 0;
                return false;
            }
        }

    }

    internal class BrowserRenderProcessHandler : CefRenderProcessHandler
    {
        protected override void OnWebKitInitialized()
        {
            base.OnWebKitInitialized();
            CefRuntime.RegisterExtension("AddScriptToExecuteOnDocumentCreatedAsync", AppBuilder.FileProvider.GetEmbeddedManifestContent("blazor.document.js"), new CefV8HandlerImpl());
        }

        public class CefV8HandlerImpl : CefV8Handler
        {
            protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
            {
                returnValue = null;
                exception = null;
                return false;
            }
        }
    }
}
