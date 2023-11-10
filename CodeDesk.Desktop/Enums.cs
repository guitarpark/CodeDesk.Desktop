using System.Drawing;
using System.Runtime.InteropServices;

namespace CodeDesk.Desktop
{
    /// <summary>
    /// 窗体状态
    /// </summary>
    public enum WindowState
    {
        Hide = -1,
        Normal = 0,
        Minimize = 1,
        Maximize = 2
    }
    /// <summary>
    /// 浏览器内容类别
    /// </summary>
    public enum WebAppType
    {
        Blazor = 0,
        Local = 1,
        Http = 2,
    }


    /// <summary>
    /// window相关枚举
    /// </summary>
    public enum SystemCommandType
    {
        /// <summary>
        /// 拖动
        /// </summary>
        Drag = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        Close = 2,
        /// <summary>
        /// 最小化
        /// </summary>
        Minimize = 3,
        /// <summary>
        /// 还原
        /// </summary>
        Restore = 4,
        /// <summary>
        /// 最大化
        /// </summary>
        Maximized = 5,
        /// <summary>
        /// 浏览器版本
        /// </summary>
        BrowserVersion = 6,
        /// <summary>
        /// 错误对话框
        /// </summary>
        ShowError = 7,
        /// <summary>
        /// 对话框
        /// </summary>
        ShowMessage = 8,
        /// <summary>
        /// 打开系统浏览器
        /// </summary>
        SystemBroswer = 9,
        /// <summary>
        /// 选择目录
        /// </summary>
        OpenDirectory = 10,
        OpenFile = 11,
        OpenFiles = 12,
        SaveFile = 13
    }

    public enum MessageBoxButton
    {
        OK = 0,
        OKCancel = 1,
        YesNo = 2,
        YesNoCancel = 3
    }
    public enum MessageResult
    {
        OK = 0,
        Cancel = 1,
        Yes = 2,
        No = 3
    }

    //   0　　　　　　　　　　　 //对话框建立失败
    //IDOK = 1　　　　　　　　//按确定按钮
    //IDCANCEL = 2　　　　　　//按取消按钮
    //IDABOUT = 3　　　　　　 //按异常终止按钮
    //IDRETRY = 4　　　　　　 //按重试按钮
    //IDIGNORE = 5　　　　　　//按忽略按钮
    //IDYES = 6　　　　　　　 //按是按钮
    //IDNO = 7　　　　　　　　//按否按钮
}
