using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.NativeHost.macOSHost
{
    internal class MessageBox : IMessageBox
    {
        public MessageResult ShowError(IntPtr? handle, string message, string caption = "出错了")
        {
            return MessageResult.Cancel;
        }

        public MessageResult ShowError(IntPtr? handle, string message, MessageBoxButton button, string caption = "出错了")
        {
            return MessageResult.Cancel;
        }

        public MessageResult ShowMessage(IntPtr? handle, string message, string caption = "确认")
        {
            return MessageResult.Cancel;
        }

        public MessageResult ShowMessage(IntPtr? handle, string message, MessageBoxButton button, string caption = "确认")
        {

            return MessageResult.Cancel;
        }
     
    }
}