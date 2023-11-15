using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.Extensions
{
    public static class RuntimePlatformExtensions
    {
        public static RuntimePlatform DetectPlatform()
        {
            var platformId = Environment.OSVersion.Platform;

            if (platformId == PlatformID.MacOSX)
                return RuntimePlatform.macOS;

            int p = (int)platformId;
            if ((p == 4) || (p == 128))
                return IsRunningOnMac() ? RuntimePlatform.macOS : RuntimePlatform.Linux;

            return RuntimePlatform.Windows;
        }


        private static bool IsRunningOnMac()
        {
            IntPtr buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal(8192);
                // This is a hacktastic way of getting sysname from uname ()
                if (uname(buf) == 0)
                {
                    string os = Marshal.PtrToStringAnsi(buf);
                    if (os == "Darwin")
                        return true;
                }
            }
            catch { }
            finally
            {
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal(buf);
            }

            return false;
        }
        [DllImport("libc")]
        private static extern int uname(IntPtr buf);

        public static bool Is64Bit()
        {
            return Environment.Is64BitOperatingSystem;
        }
    }
    public enum RuntimePlatform
    {
        Windows,
        Linux,
        macOS,
    }
}
