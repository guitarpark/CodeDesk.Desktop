using System.Drawing;
using System.Reflection;

namespace CodeDesk.Desktop.NativeHost.WindowHost
{
    internal class NativeSplashWindow : NativeWindowBase, ISplashWindow
    {
        SplashConfig Option;
        WindowConfig WindowOption;
        internal NativeSplashWindow(SplashConfig option, WindowConfig windowOption)
        {
            base.Size = option.Size;
            base.StartupCenter = true;

            this.Option = option;
            this.WindowOption = windowOption;
        }
        public override void Create()
        {
            base.Create();
            Handle.DrawImage(Application.FileProvider.GetImage(this.Option.Splash), 0, 0, this.Option.Size.Width, this.Option.Size.Height);
        }
        public override void Show()
        {
            CheckRuntime();
            base.Show();
        }
        public override IntPtr WndProc(IntPtr hwnd, Win32Api.WM message, IntPtr wParam, IntPtr lParam)
        {
            switch (message)
            {
                case Win32Api.WM.GETICON:
                    {
                        return IntPtr.Zero;
                    }
                case Win32Api.WM.CLOSE:
                    {
                        Win32Api.DestroyWindow(hwnd);
                        Handle = IntPtr.Zero;
                        var main = new NativeMainWindow(this.WindowOption);
                        main.Create();
                        main.Show();
                        Win32Api.PostQuitMessage(0);
                        break;
                    }
                case Win32Api.WM.NCHITTEST:
                    {
                        var result = Win32Api.DefWindowProcW(hwnd, message, wParam, lParam);
                        if (Win32Api.BorderHitTestResults.Contains((Win32Api.HT)result))
                        {
                            return result;
                        }
                        return (IntPtr)Win32Api.HT.CAPTION;
                    }
                case Win32Api.WM.ERASEBKGND:
                    {
                        return IntPtr.Zero;
                    }
                case Win32Api.WM.NCACTIVATE:
                    {
                        return Win32Api.DefWindowProcW(hwnd, message, wParam, new IntPtr(-1));
                    }
            }
            return (Win32Api.DefWindowProcW(hwnd, message, wParam, lParam));
        }
        internal override Win32Api.WS GetStyle()
        {
            return Win32Api.WS.CLIPCHILDREN | Win32Api.WS.CLIPSIBLINGS | Win32Api.WS.POPUP | Win32Api.WS.VISIBLE;
        }
        int loadingWidth = 0;
        async Task CheckRuntime()
        {
            int loadingStepWidth = this.Option.Loading.Width / 100;
            var runtime = await new WebViewHost.WebView2Host.WebView2().CheckWebView(async (progress) =>
            {
                loadingWidth = loadingStepWidth * (int)progress;

                using (var graphics = Graphics.FromHwnd(Handle))
                {
                    graphics.FillRectangle(Brushes.Green, this.Option.Loading.Left, this.Option.Loading.Top, loadingWidth, this.Option.Loading.Height);
                }
            });
            if (!runtime.Succeed)
            {
                Application.MessageBox.ShowError(Handle, $"初始化运行环境失败！{runtime.Message}");
                base.ExitApplication();
            }
            ShowSplashScreen();
        }

        Timer timer;
        void ShowSplashScreen()
        {
            int loadingInterval = this.Option.Loading.Delayed / 100;
            int loadingStepWidth = this.Option.Loading.Width / 100;

            timer = new Timer((object state) =>
            {
                if (loadingWidth >= this.Option.Loading.Width)
                {
                    timer.Dispose();
                    base.Close();
                }
                else
                {
                    loadingWidth += loadingStepWidth;
                    using (var graphics = Graphics.FromHwnd(Handle))
                    {
                        graphics.FillRectangle(Brushes.Green, this.Option.Loading.Left, this.Option.Loading.Top, loadingWidth, this.Option.Loading.Height);
                    }
                }
            }, null, 0, loadingInterval);

        }

        internal override string GetClassName()
        {
            return $"{Assembly.GetEntryAssembly().GetName().Name}.{this.GetType().Name}";
        }
    }

}

