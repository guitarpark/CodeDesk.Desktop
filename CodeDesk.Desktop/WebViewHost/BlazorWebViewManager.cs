using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Channels;

namespace CodeDesk.Desktop.WebViewHost
{
    internal class BlazorWebViewManager : WebViewManager
    {
        IFileProvider fileProvider;
        IWebView webView;
        BlazorDispatcher dispatcher;
        Task handleMessageTask;
        Channel<string> messageQueue;
        IWindow window;
        public BlazorWebViewManager(IWindow window, IWebView webView, IServiceProvider services, BlazorDispatcher dispatcher, IFileProvider fileProvider,
           JSComponentConfigurationStore jsComponents, SchemeConfig config)
           : base(services, dispatcher, config.AppOriginUri, fileProvider, jsComponents, config.HomePagePath)
        {

            this.window = window;
            this.fileProvider = fileProvider;
            this.webView = webView;
            this.dispatcher = dispatcher;
            messageQueue = Channel.CreateUnbounded<string>(new UnboundedChannelOptions() { SingleReader = true, SingleWriter = false, AllowSynchronousContinuations = false });
            handleMessageTask = Task.Factory.StartNew(MessageReadProgress, TaskCreationOptions.LongRunning);
        }

        protected override async void NavigateCore(Uri absoluteUri)
        {
            await dispatcher.InvokeAsync(() =>
            {
                webView.Navigate(absoluteUri.ToString());
            });
        }
        protected override void SendMessage(string message)
        {
            messageQueue.Writer.TryWrite(message);
        }
        async Task MessageReadProgress()
        {
            var reader = messageQueue.Reader;
            try
            {
                while (true)
                {
                    var message = await reader.ReadAsync();
                    await dispatcher.InvokeAsync(() => webView.SendWebMessage(message));
                }
            }
            catch (Exception ex)
            {
                var aaa = ex;
            }
        }

        protected override ValueTask DisposeAsyncCore()
        {
            try
            {
                messageQueue.Writer.Complete();
            }
            catch (Exception)
            {

            }

            handleMessageTask.Wait();
            handleMessageTask.Dispose();

            return base.DisposeAsyncCore();
        }

        #region 给各个平台浏览器 调用
        public void OnMessageReceived(string source, string message)
        {
            //检查是否是系统命令
            var systemCommand = SystemCommand.SystemMessageHandler(window,webView, message);
            if (systemCommand)
                return;
            MessageReceived(new Uri(source), message);
            //处理系统内部事件
          Application.MessageReceivedHandler?.Invoke(message);
        }

        public (Stream Content, string Type) OnResourceRequested(SchemeConfig config,string url)
        {
            var filePath = config.AppOriginUri.ToRelativeUrl(url, config.HomePage);
            IFileInfo fileInfo = fileProvider.GetFileInfo(filePath);
            if (fileInfo.Exists)
            {
                return (fileInfo.CreateReadStream(), filePath.GetContentType().contentType);
            }
            else
            {
                return (new MemoryStream(Encoding.UTF8.GetBytes("no content " + url)), "text/plain");
            }
        }
        #endregion
    }
}
