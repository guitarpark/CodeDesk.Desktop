using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop
{
    internal interface IWebView
    {
        IntPtr Handle { get; set; }
        Task<(bool Succeed, string Message)> CheckWebView(Action<double> progress);
        /// <summary>
        /// 版本号
        /// </summary>
        string Version { get; }
        void Navigate(string url);
        /// <summary>
        /// 调试模式
        /// </summary>
        void SetDebug(bool debug);
        void SetWebAppType(WebAppType type);
        void SendWebMessage(string message);
        void SendWebMessage(SystemMessageModel message);
        /// <summary>
        /// 初始化WebView
        /// </summary>
        Task Initialization(IWindow window, int width, int height, string url, Type blazorComponent, string blazorSelector);
        void SizeChange(IntPtr handle, int width, int height);
    }
}
