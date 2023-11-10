using Microsoft.Extensions.FileProviders;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.WindowHost
{
    internal static class WindowExtensions
    {
        internal static void ForceRedraw(IntPtr hwnd)
        {
            Win32Api.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.FRAMECHANGED | Win32Api.SWP.NOSIZE | Win32Api.SWP.NOMOVE | Win32Api.SWP.NOREDRAW | Win32Api.SWP.NOZORDER | Win32Api.SWP.NOOWNERZORDER);
        }
        internal static Size GetScreenSize()
        {
            IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
            Size size = new Size();
            size.Width = Win32Api.GetDeviceCaps(hdc, Win32Api.DeviceCapability.HORZRES);
            size.Height = Win32Api.GetDeviceCaps(hdc, Win32Api.DeviceCapability.VERTRES);
            Win32Api.ReleaseDC(IntPtr.Zero, hdc);
            return size;
        }
        internal static Point GetPostion(this Size size, Point postion, bool center)
        {
            var screen = GetScreenSize();
            if (center)
            {

                postion.X = (screen.Width - size.Width) / 2;
                postion.Y = (screen.Height - size.Height) / 2;
            }
            return postion;
        }

        internal static IntPtr GetWin32Color(this string hex)
        {
            return Win32Api.CreateSolidBrush((uint)ColorTranslator.ToWin32(ColorTranslator.FromHtml(hex)));
        }
        internal static Win32Api.EX GetExStyles(this WindowConfig options)
        {
            return Win32Api.EX.WINDOWEDGE | Win32Api.EX.APPWINDOW;
        }

        internal static IntPtr GetIconHandle(this IFileProvider fileProvider, string path)
        {
            if (string.IsNullOrEmpty(path))
                return IntPtr.Zero;
            var stream = fileProvider.GetFileInfo(path).CreateReadStream();
            if (stream == null)
                return IntPtr.Zero;
            return new Bitmap(stream).GetHicon();
        }
        internal static Bitmap GetImage(this IFileProvider fileProvider, string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            var stream = fileProvider.GetFileInfo(path).CreateReadStream();
            if (stream == null)
                return null;
            return new Bitmap(stream);
        }
        internal static void DrawImage(this IntPtr handle, Image image, int left, int top, int width, int height)
        {
            using (Graphics g = Graphics.FromHwnd(handle))
            {
                g.DrawImage(image, new Rectangle(left, top, width, height));
            }
        }
        internal static (int Width, int Height) ParseSize(this IntPtr LParam)
        {
            int width = (int)(LParam.ToInt64() & 0xFFFF);
            int height = (int)((LParam.ToInt64() >> 16) & 0xFFFF);
            return (width, height);
        }
    }
}
