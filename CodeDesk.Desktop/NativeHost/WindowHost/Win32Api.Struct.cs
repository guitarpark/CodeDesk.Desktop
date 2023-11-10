using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.WindowHost
{
    internal static partial class Win32Api
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct WNDCLASS
        {
            public uint style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;

            [MarshalAs(UnmanagedType.LPWStr)] public string lpszMenuName;

            [MarshalAs(UnmanagedType.LPWStr)] public string lpszClassName;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MSG
        {
            public IntPtr hwnd;
            public WM message;
            public IntPtr wParam;
            public IntPtr lParam;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct NcCalcSizeParams
        {
            public NcCalcSizeRegionUnion Region;
            public WINDOWPOS* Position;
        }
        [StructLayout(LayoutKind.Explicit)]
        internal struct NcCalcSizeRegionUnion
        {
            [FieldOffset(0)] public NcCalcSizeInput Input;
            [FieldOffset(0)] public NcCalcSizeOutput Output;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NcCalcSizeInput
        {
            public RECT TargetWindowRect;
            public RECT CurrentWindowRect;
            public RECT CurrentClientRect;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NcCalcSizeOutput
        {
            public RECT TargetClientRect;
            public RECT DestRect;
            public RECT SrcRect;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public SWP flags;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int X;
            public int Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width()
            {
                return this.Right - this.Left;
            }
            public int Height()
            {
                return this.Bottom-this.Top;
            }
        }

    }
}