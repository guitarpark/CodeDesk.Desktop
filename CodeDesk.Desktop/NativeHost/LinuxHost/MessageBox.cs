using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.NativeHost.LinuxHost
{
    internal class MessageBox : IMessageBox
    {
        public MessageResult ShowError(IntPtr? handle, string message, string caption = "出错了")
        {
            return ShowError(handle, message, MessageBoxButton.OK, caption);
        }

        public MessageResult ShowError(IntPtr? handle, string message, MessageBoxButton button, string caption = "出错了")
        {
            IntPtr dialog = GtkApi.gtk_message_dialog_new(handle.GetValueOrDefault(IntPtr.Zero), 0, (int)MessageType.Error, ToMessageButton(button), message);
            GtkApi.gtk_window_set_title(dialog, caption);
            int response = GtkApi.gtk_dialog_run(dialog);
            GtkApi.gtk_widget_destroy(dialog);
            return ToMessageResult(response);
        }

        public MessageResult ShowMessage(IntPtr? handle, string message, string caption = "确认")
        {
            return ShowMessage(handle, message, MessageBoxButton.OK, caption);
        }

        public MessageResult ShowMessage(IntPtr? handle, string message, MessageBoxButton button, string caption = "确认")
        {
            IntPtr dialog = GtkApi.gtk_message_dialog_new(handle.GetValueOrDefault(IntPtr.Zero), 0, (int)MessageType.Info, ToMessageButton(button), message);
            GtkApi.gtk_window_set_title(dialog, caption);
            int response = GtkApi.gtk_dialog_run(dialog);
            GtkApi.gtk_widget_destroy(dialog);
            return ToMessageResult(response);
        }
        //None  -1;Reject -2;Accept -3;DeleteEvent -4;Ok -5;Cancel -6;Close -7;Yes -8;No -9;Apply -10;Help -11
        MessageResult ToMessageResult(int result)
        {
            switch (result)
            {
                case -6:
                    return MessageResult.Cancel;
                case -8:
                    return MessageResult.Yes;
                case -9:
                    return MessageResult.No;
                default:
                    return MessageResult.OK;
            }
        }

        int ToMessageButton(MessageBoxButton buttonsType)
        {
            switch (buttonsType)
            {
                case MessageBoxButton.OKCancel:
                    return (int)ButtonsType.OkCancel;
                case MessageBoxButton.YesNo:
                case MessageBoxButton.YesNoCancel:
                    return (int)ButtonsType.YesNo;
                default:
                    return (int)ButtonsType.Ok;
            }
        }
        private enum MessageType
        {
            Info,
            Warning,
            Question,
            Error,
            Other
        }

        [Flags]
        private enum ButtonsType
        {
            None,
            Ok,
            Close,
            Cancel,
            YesNo,
            OkCancel
        }
    }
}