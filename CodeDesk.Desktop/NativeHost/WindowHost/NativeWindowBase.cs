using System.Drawing;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.WindowHost
{
    internal abstract class NativeWindowBase
    {
        internal Size? MaximumSize { get; set; }
        internal Size? MinimumSize { get; set; }
        internal Size Size { get; set; }
        public IntPtr Handle { get; set; }
        internal WindowState WindowState { get; set; }
        internal bool Chromeless { get; set; }
        internal bool CanReSize { get; set; }
        internal bool CanMinMax { get; set; }
        internal bool StartupCenter { get; set; }
        internal int Left { get; set; }
        internal int Top { get; set; }
        internal event EventHandler<SizeChangeEventArgs> OnSizeChange;

        int managedThreadId;
        internal NativeWindowBase()
        {
            this.managedThreadId = Environment.CurrentManagedThreadId;
        }
        private static WndProcDelegate windowProc;

        delegate IntPtr WndProcDelegate(IntPtr hWnd, Win32Api.WM msg, IntPtr wParam, IntPtr lParam);

        internal abstract string GetClassName();
        public virtual void Create()
        {
            windowProc = WndProc;
            var windClass = new Win32Api.WNDCLASS
            {
                lpszClassName = GetClassName(),
                hbrBackground = Application.BackgroundColor.GetWin32Color(),
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(windowProc),
                cbClsExtra = 0,
                cbWndExtra = 0,
                style = 0x0003,
                hInstance = Win32Api.GetConsoleWindow(),
                lpszMenuName = null,
                hCursor = Win32Api.LoadCursorW(IntPtr.Zero, (IntPtr)Win32Api.CursorResource.IDC_ARROW),
                hIcon = Application.FileProvider.GetIconHandle(Application.Icon)
            };
            if (Win32Api.RegisterClassW(ref windClass) == 0)
            {
                throw new Exception("初始化窗体失败!");
            }


            if (StartupCenter)
            {
                var screen = GetScreenSize();
                Left = (screen.Width - Size.Width) / 2;
                Top = (screen.Height - Size.Height) / 2;
            }
            Handle = Win32Api.CreateWindowExW((Win32Api.EX.WINDOWEDGE | Win32Api.EX.APPWINDOW), windClass.lpszClassName, Application.AppName, GetStyle(), Left, Top, Size.Width, Size.Height, IntPtr.Zero, IntPtr.Zero, windClass.hInstance, null);
            if (Handle == IntPtr.Zero)
            {
                throw new Exception("创建窗体失败!");
            }

            Win32Api.SetWindowTextW(Handle, Application.AppName);
            Win32Api.UpdateWindow(Handle);
        }
        internal void RunMessageLoop()
        {
            Win32Api.MSG message;
            while (Win32Api.GetMessageW(out message, IntPtr.Zero, 0, 0))
            {
                Win32Api.TranslateMessage(ref message);
                Win32Api.DispatchMessageW(ref message);
            }
        }
        internal Size GetScreenSize()
        {
            IntPtr hdc = Win32Api.GetDC(IntPtr.Zero);
            Size size = new Size();
            size.Width = Win32Api.GetDeviceCaps(hdc, Win32Api.DeviceCapability.HORZRES);
            size.Height = Win32Api.GetDeviceCaps(hdc, Win32Api.DeviceCapability.VERTRES);
            Win32Api.ReleaseDC(IntPtr.Zero, hdc);
            return size;
        }
        public virtual IntPtr WndProc(IntPtr hwnd, Win32Api.WM message, IntPtr wParam, IntPtr lParam)
        {
            switch (message)
            {
                case Win32Api.WM.GETMINMAXINFO:
                    {
                        var mmi = (Win32Api.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(Win32Api.MINMAXINFO));
                        // 设置最小宽度和最小高度
                        var maxInfo = MaximumSize.GetValueOrDefault(new Size(0, 0));
                        var minInfo = MinimumSize.GetValueOrDefault(new Size(0, 0));
                        if (minInfo.Width > 0)
                            mmi.ptMinTrackSize.X = minInfo.Width;
                        if (minInfo.Height > 0)
                            mmi.ptMinTrackSize.Y = minInfo.Height;

                        if (maxInfo.Width > 0)
                            mmi.ptMaxTrackSize.X = maxInfo.Width;
                        if (maxInfo.Height > 0)
                            mmi.ptMaxTrackSize.Y = maxInfo.Height;
                        Marshal.StructureToPtr(mmi, lParam, true);
                        break;
                    }
                case Win32Api.WM.APP:
                    {
                        if (wParam != IntPtr.Zero)
                        {
                            Action action = (Action)Marshal.GetDelegateForFunctionPointer(wParam, typeof(Action));
                            action.Invoke();
                        }

                        return IntPtr.Zero;
                    }
                case Win32Api.WM.SIZE:
                    {
                        var size = GetClientSize();
                        OnSizeChange?.Invoke(this, new SizeChangeEventArgs() { Width = size.Width(), Height = size.Height() });
                        WindowExtensions.ForceRedraw(hwnd);
                        break;
                    }
                case Win32Api.WM.CLOSE:
                    {
                        Handle = IntPtr.Zero;
                        Win32Api.PostQuitMessage(0);
                        Environment.Exit(0);
                        return IntPtr.Zero;
                    }

            }
            return (Win32Api.DefWindowProcW(hwnd, message, wParam, lParam));
        }

        public virtual void Show()
        {
            switch (WindowState)
            {
                case WindowState.Maximize:
                    Maximize();
                    break;
                case WindowState.Minimize:
                    Minimize();
                    break;
                default:
                    Win32Api.ShowWindow(Handle, Win32Api.SW.SHOW);
                    break;

            }
            RunMessageLoop();
        }
        public virtual void Close()
        {
            Win32Api.PostMessage(Handle, (uint)Win32Api.WM.CLOSE, IntPtr.Zero, IntPtr.Zero);
        }
        public bool CheckAccess()
        {
            return Environment.CurrentManagedThreadId == managedThreadId;
        }
        public Task InvokeAsync(Action workItem)
        {
            if (Environment.CurrentManagedThreadId == managedThreadId)
                workItem();
            else
            {
                IntPtr actionPtr = Marshal.GetFunctionPointerForDelegate(workItem);
                Win32Api.PostMessage(Handle, (uint)Win32Api.WM.APP, actionPtr, IntPtr.Zero);
            }

            return Task.CompletedTask;
        }
        public async Task InvokeAsync(Func<Task> workItem)
        {

            if (Environment.CurrentManagedThreadId == managedThreadId)
                await workItem();
            else
            {
                IntPtr actionPtr = Marshal.GetFunctionPointerForDelegate(workItem);
                Win32Api.PostMessage(Handle, (uint)Win32Api.WM.APP, actionPtr, IntPtr.Zero);
            }
        }

        public virtual void Maximize()
        {
            Win32Api.ShowWindow(Handle, Win32Api.SW.MAXIMIZE);
            if (Chromeless)
            {
                int screenWidth = Win32Api.GetSystemMetrics(Win32Api.SystemMetric.CXSCREEN);
                int screenHeight = Win32Api.GetSystemMetrics(Win32Api.SystemMetric.CYSCREEN);
                int captionHeight = Win32Api.GetSystemMetrics(Win32Api.SystemMetric.CYCAPTION);

                int frameWidth = Win32Api.GetSystemMetrics(Win32Api.SystemMetric.CXSIZEFRAME);
                int frameHeight = Win32Api.GetSystemMetrics(Win32Api.SystemMetric.CYSIZEFRAME);

                int width = screenWidth + (frameWidth * 4);
                int height = screenHeight - captionHeight + (frameHeight * 2);

                Win32Api.SetWindowPos(Handle, IntPtr.Zero, -frameWidth * 2, 0, width, height, Win32Api.SWP.SHOWWINDOW);
            }
        }
        public virtual void Restore()
        {
            Win32Api.ShowWindow(Handle, Win32Api.SW.RESTORE);
        }
        public virtual void Minimize()
        {
            Win32Api.ShowWindow(Handle, Win32Api.SW.MINIMIZE);
        }
        public virtual void Drag(int left, int top)
        {
            Win32Api.RECT rect;
            if (Win32Api.GetWindowRect(Handle, out rect))
            {
                try
                {
                    Win32Api.SetWindowPos(Handle, IntPtr.Zero, (rect.Left + left), (rect.Top + top), (rect.Right - rect.Left), (rect.Bottom - rect.Top), 0);
                }
                catch
                {
                    //TODO:偶尔会产生System.ExecutionEngineException，后续修改
                }
            }
        }
        internal void DrawImage(Image image, int left, int top, int width, int height)
        {
            using (Graphics g = Graphics.FromHwnd(Handle))
            {
                g.DrawImage(image, new Rectangle(left, top, width, height));
            }
        }

        internal void ExitApplication()
        {
            Win32Api.PostQuitMessage(0);
            Environment.Exit(0);
        }

        internal virtual Win32Api.WS GetStyle()
        {
            Win32Api.WS styles;
            if (Chromeless)
                styles = Win32Api.WS.POPUPWINDOW | Win32Api.WS.CLIPCHILDREN | Win32Api.WS.CLIPSIBLINGS | Win32Api.WS.SIZEBOX | Win32Api.WS.MINIMIZEBOX | Win32Api.WS.MAXIMIZEBOX;
            else
                styles = Win32Api.WS.OVERLAPPEDWINDOW | Win32Api.WS.CLIPCHILDREN | Win32Api.WS.CLIPSIBLINGS;
            if (CanReSize == false)
            {
                styles &= ~Win32Api.WS.MAXIMIZEBOX & ~Win32Api.WS.THICKFRAME;
            }
            if (CanMinMax == false)
            {
                styles &= ~Win32Api.WS.MINIMIZEBOX & ~Win32Api.WS.MAXIMIZEBOX;
            }
            return styles;
        }

        internal Win32Api.RECT GetClientSize()
        {
            Win32Api.RECT rect;
            Win32Api.GetClientRect(Handle, out rect);
            return rect;
        }
        internal int GetTopFrameHeight()
        {
            if (!Chromeless || WindowState == WindowState.Maximize)
                return 0;
            return Win32Api.GetSystemMetrics(Win32Api.SystemMetric.CYCAPTION) - Win32Api.GetSystemMetrics(Win32Api.SystemMetric.CYMENU) + Win32Api.GetSystemMetrics(Win32Api.SystemMetric.CXPADDEDBORDER);
        }
    }
}
