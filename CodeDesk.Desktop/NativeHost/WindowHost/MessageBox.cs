namespace CodeDesk.Desktop.NativeHost.WindowHost
{
    internal class MessageBox : IMessageBox
    {
        public MessageResult ShowMessage(IntPtr? handle, string message, string caption = "确认")
        {
            handle ??= Win32Api.GetConsoleWindow();
            return Win32ToMessageResult(Win32Api.MessageBox(handle.Value, message, caption, 0 | 64));
        }
        public MessageResult ShowMessage(IntPtr? handle, string message, MessageBoxButton button, string caption = "确认")
        {
            return Win32ToMessageResult(Win32Api.MessageBox(Win32Api.GetConsoleWindow(), message, caption, (int)button | 64));
        }
        public MessageResult ShowError(IntPtr? handle, string message, string caption = "出错了")
        {
            return Win32ToMessageResult(Win32Api.MessageBox(Win32Api.GetConsoleWindow(), message, caption, 0 | 16));
        }
        public MessageResult ShowError(IntPtr? handle, string message, MessageBoxButton button, string caption = "出错了")
        {
            return Win32ToMessageResult(Win32Api.MessageBox(Win32Api.GetConsoleWindow(), message, caption, (int)button | 16));
        }
        MessageResult Win32ToMessageResult(int result)
        {
            switch (result)
            {
                case 2:
                    return MessageResult.Cancel;
                case 6:
                    return MessageResult.Yes;
                case 7:
                    return MessageResult.No;
                default:
                    return MessageResult.OK;
            }
        }
    }
}
