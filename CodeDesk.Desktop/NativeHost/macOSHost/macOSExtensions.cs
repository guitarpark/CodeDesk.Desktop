using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.macOSHost;

public static class macOSExtensions
{
    public static IntPtr TomacOSIntptr(this string text)
    {
        GCHandle _titleHandle;
        try
        {
            if (System.Text.Encoding.UTF8.GetByteCount(text) != text.Length)
            {
                byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                _titleHandle = GCHandle.Alloc(utf8Bytes, GCHandleType.Pinned);
                return _titleHandle.AddrOfPinnedObject();
            }

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(text);
            _titleHandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            return _titleHandle.AddrOfPinnedObject();
        }
        catch { }

        return IntPtr.Zero;
    }
}