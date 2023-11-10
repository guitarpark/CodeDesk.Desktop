using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.LinuxHost
{
    internal static class LinuxExtensions
    {
        internal static IntPtr GetPixbuf(this byte[] imageData)
        {
            var imagePtr = Marshal.UnsafeAddrOfPinnedArrayElement(imageData, 0);
            var stream = GtkApi.g_memory_input_stream_new_from_data(imagePtr, imageData.Length, IntPtr.Zero);
            var pixbuf = GtkApi.gdk_pixbuf_new_from_stream(stream, IntPtr.Zero,IntPtr.Zero);
            GtkApi.g_input_stream_close(stream, IntPtr.Zero, IntPtr.Zero);
            return pixbuf;
        }
        internal static IntPtr GetImageIntPtr(this Stream stream)
        {
            var pixbuf = stream.StreamToByte().GetPixbuf();
            return GtkApi.gtk_image_new_from_pixbuf(pixbuf);
        }
        internal static void InitGtk()
        {
            GtkApi.XInitThreads();
            IntPtr argv = IntPtr.Zero;
            int argc = 0;
            GtkApi.gtk_init(ref argc, ref argv);
        }

        internal static IntPtr ToGdkRGBAIntPtr(this string color)
        {
            var rgb = color.HtmlColorToRgb();
            return (new GtkApi.GdkRGBA() { red = rgb.R, green = rgb.G, blue = rgb.B,alpha = rgb.A}).StructToIntPtr();
        }
        
        internal delegate bool InvokeCallbackWrapperDelegate(IntPtr data);
        internal static bool InvokeCallback(IntPtr data)
        {
            GCHandle handle = GCHandle.FromIntPtr(data);
            var waitInfo = (GtkApi.InvokeWaitInfo)handle.Target;
            waitInfo.callback?.Invoke();
            return false;
        }
        internal static bool InvokeCallbackTask(IntPtr data)
        {
            GCHandle handle = GCHandle.FromIntPtr(data);
            GtkApi.InvokeWaitInfo waitInfo = (GtkApi.InvokeWaitInfo)handle.Target;
            waitInfo.callback?.Invoke();
            return false;
        }
    }
}
