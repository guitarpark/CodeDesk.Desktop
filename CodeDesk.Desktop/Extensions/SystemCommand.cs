using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;

namespace CodeDesk.Desktop.Extensions
{
    public static class SystemCommand
    {
        public static async void WindowCommand(this IJSRuntime JSRuntime, SystemCommandType command)
        {
            await JSRuntime.InvokeVoidAsync("window.external.systemCommand", command.ToString());
        }
        public static async void SystemBroswer(this IJSRuntime JSRuntime, string url)
        {
            await JSRuntime.InvokeVoidAsync("window.external.systemCommand", SystemCommandType.SystemBroswer.ToString(),url);
        }
        public static async void CustomMessage(this IJSRuntime JSRuntime, string message)
        {
            await JSRuntime.InvokeVoidAsync("window.external.sendMessage", message);
        }
        public static async void InitDrag(this IJSRuntime JSRuntime, string selector)
        {
            await JSRuntime.InvokeVoidAsync("window.external.drag", selector);
        }
        public static async void ReceiveMessage(this IJSRuntime JSRuntime,Action<SystemMessageModel> Data)
        {
            //var fff= await JSRuntime.InvokeAsync("window.external.drag", selector)
        }
        public static async void Foucs(this IJSRuntime JSRuntime, string selector)
        {
            await JSRuntime.InvokeVoidAsync("window.external.focus", selector);
        }
        public static async void RegisterMessage<T>(this IJSRuntime JSRuntime,T value, string function)where T : class
        {
            await JSRuntime.InvokeVoidAsync("window.external.blazorMessage", DotNetObjectReference.Create(value), function);
        }

        internal static bool SystemMessageHandler(this IWindow window, IWebView webview, string message)
        {
            if (message.StartsWith("{\"SystemCommand\""))
            {
                var data = message.FromJson<SystemMessageModel>();
                switch (data.SystemCommand)
                {
                    case SystemCommandType.Minimize:
                        {
                            window.Minimize();
                            return true;
                        }
                    case SystemCommandType.Maximized:
                        {
                            window.Maximize();
                            return true;
                        }
                    case SystemCommandType.Restore:
                        {
                            window.Restore();
                            return true;
                        }
                    case SystemCommandType.Close:
                        {
                            window.Close();
                            return true;
                        }
                    case SystemCommandType.Drag:
                        {
                            var value = data.Data.Split(":");
                            window.Drag(value[0].ToInt(), value[1].ToInt());
                            return true;
                        }
                    case SystemCommandType.BrowserVersion:
                        {
                            webview.SendWebMessage(new SystemMessageModel() { SystemCommand = SystemCommandType.BrowserVersion, Data = webview.Version });
                            return true;
                        }
                    case SystemCommandType.ShowError:
                        {
                            var option = data.Data.FromJson<MessageBoxModel>();
                            var result = Application.MessageBox.ShowError(window.Handle, option.Message, option.Button, option.Caption);

                            webview.SendWebMessage(new SystemMessageModel() { SystemCommand = SystemCommandType.ShowError, Data = result.ToString() });
                            return true;
                        }
                    case SystemCommandType.ShowMessage:
                        {
                            var option = data.Data.FromJson<MessageBoxModel>();
                            var result = Application.MessageBox.ShowMessage(window.Handle, option.Message, option.Button, option.Caption);

                            webview.SendWebMessage(new SystemMessageModel() { SystemCommand = SystemCommandType.ShowError, Data = result.ToString() });
                            return true;
                        }
                    case SystemCommandType.SystemBroswer:
                        {
                            window.OpenSystemBroswer(data.Data.FromJson());
                            return true;
                        }
                    case SystemCommandType.OpenDirectory:
                        {
                            var result = Application.FileDialog.OpenDirectory(data.Data.FromJson());
                            webview.SendWebMessage(new SystemMessageModel()
                            {
                                SystemCommand = SystemCommandType.OpenDirectory,
                                Data = new
                                {
                                    Selected = result.Selected,
                                    Path = result.path
                                }.ToJson()
                            });
                            return true;
                        }
                    case SystemCommandType.OpenFile:
                        {
                            var model = data.Data.FromJson<OpenFileModel>();
                            var result = Application.FileDialog.OpenFile(model.InitialDir, model.Filters);
                            webview.SendWebMessage(new SystemMessageModel()
                            {
                                SystemCommand = SystemCommandType.OpenFile,
                                Data = new
                                {
                                    Selected = result.Selected,
                                    FileName = result.fileName,
                                    FileFullName = result.fileFullName
                                }.ToJson()
                            });
                            return true;
                        }

                    case SystemCommandType.OpenFiles:
                        {
                            var model = data.Data.FromJson<OpenFileModel>();
                            var result = Application.FileDialog.OpenFiles(model.InitialDir, model.Filters);
                            webview.SendWebMessage(new SystemMessageModel()
                            {
                                SystemCommand = SystemCommandType.OpenFile,
                                Data = new
                                {
                                    Selected = result.Selected,
                                    Files = result.Files
                                }.ToJson()
                            });
                            return true;
                        }

                    case SystemCommandType.SaveFile:
                        {
                            var model = data.Data.FromJson<SaveFileModel>();
                            var result = Application.FileDialog.SaveFile(model.FileName,model.InitialDir, model.Filters);
                            webview.SendWebMessage(new SystemMessageModel()
                            {
                                SystemCommand = SystemCommandType.OpenFile,
                                Data = new
                                {
                                    Selected = result.Selected,
                                    FileName = result.fileName,
                                    FileFullName = result.fileFullName
                                }.ToJson()
                            });
                            return true;
                        }
                }
                return false;
            }
            return false;
        }
    }

   public class SystemMessageModel
    {
        public SystemCommandType SystemCommand { get; set; }
        public string CommandName { get { return SystemCommand.ToString(); } }
        public string Data { get; set; }
    }
    public class MessageBoxModel
    {
        public string Message { get; set; }
        public MessageBoxButton Button { get; set; } = MessageBoxButton.OK;
        public string Caption { get; set; }
    }
    public class OpenFileModel
    {
        public string InitialDir { get; set; }
        public Dictionary<string, string> Filters { get; set; }
    }
    public class SaveFileModel
    {
        public string FileName { get; set; }
        public string InitialDir { get; set; }
        public Dictionary<string, string> Filters { get; set; }
    }
}
