using System.Reflection;
using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.macOSHost
{
    internal class NativeSplashWindow :  ISplashWindow
    {
        
        internal const string libmacOSLibrary = "libmacOSLibrary.dylib";
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Create1();
        
        public static void LoadNativeHostFile()
        {

           
                string resourcePath = $"CodeDesk.Desktop.NativeHost.macOSHost.{libmacOSLibrary}";
                using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
                if (resource is not null)
                {
                    using var file = new FileStream(libmacOSLibrary, FileMode.Create, FileAccess.Write);
                    resource.CopyTo(file);
                }
        }
        internal NativeSplashWindow(SplashConfig option, WindowConfig windowOption)
        {
        }

        public void Create()
        {
            LoadNativeHostFile();
            Create1();
        }

        public void Show()
        {
        }
        public void Close()
        {

        }
    }
}