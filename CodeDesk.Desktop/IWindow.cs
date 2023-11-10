using Microsoft.Extensions.FileProviders;
using System.Drawing;

namespace CodeDesk.Desktop
{
    internal interface IWindow
    {
        void Create();
        void Show();
        IntPtr Handle { get; set; }
        bool CheckAccess();
        Task InvokeAsync(Func<Task> workItem);
        Task InvokeAsync(Action workItem);
        /// <summary>
        ///关闭
        /// </summary>
        void Close();
        /// <summary>
        /// 最大化
        /// </summary>
        void Maximize();
        /// <summary>
        /// 还原
        /// </summary>
        void Restore();
        /// <summary>
        /// 最小化
        /// </summary>
        void Minimize();
        /// <summary>
        /// 拖拽
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        void Drag(int left, int top);

        void OpenSystemBroswer(string url);
    }
}