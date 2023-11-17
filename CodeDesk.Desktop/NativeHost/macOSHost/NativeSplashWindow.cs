using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
namespace CodeDesk.Desktop.NativeHost.macOSHost
{
    internal class NativeSplashWindow : NativeWindowBase, ISplashWindow
    {
        public IntPtr Handle { get; set; }
        SplashConfig Option;
        WindowConfig WindowOption;
        internal NativeSplashWindow(SplashConfig option, WindowConfig windowOption)
        {
            macOSApi.LoadNativeHostFile();
            base.Chromeless = true;
            base.Size = option.Size;
            base.StartupCenter = true;

            this.Option = option;
            this.WindowOption = windowOption;
        }

        public override void Create()
        {
            base.Create();
        }

        public override void Show()
        {
            base.Show();
        }
        public void Close()
        {
        }
    }
}