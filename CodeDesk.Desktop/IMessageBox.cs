namespace CodeDesk.Desktop
{
    public interface IMessageBox
    {
        MessageResult ShowError(IntPtr? handle,string message, string caption = "出错了");
        MessageResult ShowError(IntPtr? handle,string message, MessageBoxButton button, string caption = "出错了");
        MessageResult ShowMessage(IntPtr? handle,string message, string caption = "确认");
        MessageResult ShowMessage(IntPtr? handle,string message, MessageBoxButton button, string caption = "确认");
    }
}