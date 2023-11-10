using System.Drawing;
using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.WindowHost
{

    internal static partial class Win32Api
    {
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateWindowExW(EX dwExStyle,string lpClassName,string lpWindowName,WS dwStyle,
           int x,int y,int nWidth,int nHeight,IntPtr handleParent,IntPtr hMenu,IntPtr hInstance,object lpParam);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr DefWindowProcW(IntPtr handle, Win32Api.WM message, IntPtr wParam, IntPtr lParam);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern ushort RegisterClassW(ref WNDCLASS lpWndClass);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool ShowWindow(IntPtr handle, SW nCmdShow);

        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool UpdateWindow(IntPtr handle);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetMessageW(out MSG lpMsg, IntPtr handle, uint wMsgFilterMin, uint wMsgFilterMax);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool TranslateMessage([In] ref MSG lpMsg);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr DispatchMessageW([In] ref MSG lpmsg);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern void PostQuitMessage(int nExitCode);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr LoadCursorW(IntPtr hInstance, IntPtr lpCursorName);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool DestroyWindow(IntPtr handle);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int SetWindowTextW(IntPtr handle, string text);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetDC(IntPtr ptr);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr ReleaseDC(IntPtr handle, IntPtr hDc);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int MessageBox(IntPtr handle, string text, string caption, int options);
        [DllImport(User32Libraries)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(IntPtr handle, IntPtr handleInsertAfter, int X, int Y, int cx, int cy, SWP uFlags =0);
        [DllImport(User32Libraries, SetLastError = true)]
        internal static extern bool PostMessage(IntPtr handle, uint Msg, IntPtr wParam, IntPtr lParam);
        internal delegate IntPtr WNDPROC(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetSystemMetrics(SystemMetric nIndex);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport(User32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport(Gdi32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetDeviceCaps(IntPtr hdc, DeviceCapability nIndex);
        [DllImport(Gdi32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateSolidBrush(uint color);


        [DllImport(Kernel32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr GetConsoleWindow();

        [DllImport(Comdlg32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetOpenFileName(ref OpenFileDialogParams param);
        [DllImport(Comdlg32Libraries, SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        internal static extern bool GetSaveFileName(ref SAVEFILENAME param);

        [DllImport(shell32Libraries, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

        [DllImport(shell32Libraries, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);
    }
}
