using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.LinuxHost
{
    internal class NativeSplashWindow : NativeWindowBase, ISplashWindow
    {
        SplashConfig Option;
        WindowConfig WindowOption;

        internal NativeSplashWindow(SplashConfig option, WindowConfig windowOption)
        {
            LinuxExtensions.InitGtk();
            base.Chromeless = true;
            base.Size = option.Size;
            base.StartupCenter = true;

            this.Option = option;
            this.WindowOption = windowOption;
        }

        public override void Create()
        {
            base.Create();
         var splashPtr = Application.FileProvider.GetFileInfo(this.Option.Splash).CreateReadStream().GetImageIntPtr();
         GtkApi.gtk_container_add(base.Handle, splashPtr);
         GtkApi.gtk_widget_show(splashPtr);
        }

        public override void Show()
        {
            base.Show();
            Task.Run(() => { showSplashScreen(); });
            base.RunMessageLoop();
            var main = new NativeMainWindow(this.WindowOption);
            main.Create();
            main.Show();
            main.RunMessageLoop();
        }

        Timer timer;

        int loadingWidth = 0;

        void showSplashScreen()
        {
            
            var color = this.Option.Loading.Color.HtmlColorToRgb();
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
                    IntPtr window = GtkApi.gtk_widget_get_window(Handle);
                    IntPtr cr = GtkApi.gdk_cairo_create(window);
                    GtkApi.cairo_rectangle(cr, this.Option.Loading.Left, this.Option.Loading.Top, loadingWidth, this.Option.Loading.Height);
                    GtkApi.cairo_set_source_rgb(cr, color.R, color.G, color.B);
                    GtkApi.cairo_fill(cr);
                }
            }, null, 0, loadingInterval);
        }
    }
}