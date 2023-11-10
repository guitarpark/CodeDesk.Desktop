using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.Models
{

    public class WindowConfig
    {
        public string Url { get; set; } = "http://localhost/index.html";
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; } = "新建跨平台程序";
        /// <summary>
        /// 屏幕居中显示
        /// </summary>
        public bool StartupCenter { get; set; } = true;
        /// <summary>
        /// 窗体样式
        /// </summary>
        public bool Chromeless { get; set; } = true;
        /// <summary>
        /// 最小尺寸
        /// </summary>
        public Size? MinimumSize { get; set; } = new Size(600, 500);
        /// <summary>
        /// 最大尺寸
        /// </summary>
        public Size? MaximumSize { get; set; }
        /// <summary>
        /// 窗体默认尺寸
        /// </summary>
        public Size Size { get; set; } = new Size() { Width = 1000, Height = 700 };
        /// <summary>
        /// 窗体状态
        /// </summary>
        public WindowState WindowState { get; set; } = WindowState.Normal;
        /// <summary>
        /// 屏幕左边距
        /// </summary>
        public int Left { get; set; } = 10;
        /// <summary>
        /// 屏幕右边距
        /// </summary>
        public int Top { get; set; } = 10;
        /// <summary>
        /// 改变大小
        /// </summary>
        public bool CanReSize { get; set; } = true;
        /// <summary>
        /// 最大化最小化
        /// </summary>
        public bool CanMinMax { get; set; } = true;
        public bool IsDebug { get; set; } = false;
        /// <summary>
        /// 浏览器内容类别
        /// </summary>
        public WebAppType WebAppType { get; set; } = WebAppType.Blazor;

        public Type BlazorComponent { get; set; }
        public string BlazorSelector { get; set; }
    }
}
