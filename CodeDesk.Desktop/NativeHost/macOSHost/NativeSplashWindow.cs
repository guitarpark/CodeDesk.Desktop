using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.macOSHost
{
    internal class NativeSplashWindow :  ISplashWindow
    {
        public IntPtr Handle { get; set; }
        SplashConfig Option;
        WindowConfig WindowOption;
        internal NativeSplashWindow(SplashConfig option, WindowConfig windowOption)
        {
            macOSApi.LoadNativeHostFile();

            this.Option = option;
            this.WindowOption = windowOption;
        }

        public void Create()
        {
            Handle = macOSApi.Create(100, 200, 400, 500, false,true);
            try
            {
                var fff = GetUTF8Title("fffff");
                macOSApi.SetTitle(Handle, GetUTF8Title("fffff")); //TODO:FixError
            }
            catch (Exception ex)
            {
                var fff = ex;
            }
        }
        private static bool TitleNonASCIIChars(string title)
        {
            return (System.Text.Encoding.UTF8.GetByteCount(title) != title.Length);
        }
        private IntPtr GetUTF8Title(string title)
        {
            GCHandle _titleHandle;
            try
            {
                if (TitleNonASCIIChars(title))
                {
                    byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(title);
                    _titleHandle = GCHandle.Alloc(utf8Bytes, GCHandleType.Pinned);
                    return _titleHandle.AddrOfPinnedObject();
                }

                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(title);
                _titleHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                return _titleHandle.AddrOfPinnedObject();
            }
            catch { }

            return IntPtr.Zero;
        }
        public void Show()
        {
            macOSApi.Show(Handle);
            macOSApi.RunMessageLoop();
        }
        public void Close()
        {
        }
    }
}