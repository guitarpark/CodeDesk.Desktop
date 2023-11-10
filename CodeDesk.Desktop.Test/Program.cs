using CodeDesk.Desktop.Models;

namespace CodeDesk.Desktop.Test
{

    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    Application.MessageBox.ShowError(IntPtr.Zero, e.ToString());
                };

                var builder = Application.Initialize();
                Application.MessageReceivedHandler = new Action<string>((message) =>
                {
                    System.Diagnostics.Debug.WriteLine("customMessage"+message);
                });
                Application.BackgroundColor = "#f5f5f5";
                Application.AppName = "新建跨平台程序";
                Application.Icon = "icon-drak.png";

                builder.RegisterResource(typeof(Program));
                var splashConfig = new SplashConfig()
                {
                    Splash = "splash.png",
                };
                var windowConfig = new WindowConfig()
                {
                    Chromeless = true,
                    MinimumSize = new System.Drawing.Size(400, 400),
                    IsDebug = true,
                    WebAppType = WebAppType.Blazor,
                    BlazorComponent = typeof(App),
                    BlazorSelector = "#app",
                    Url = "http://localhost/blazorindex.html"
                };
                builder.CreateWindow(splashConfig, windowConfig);
                builder.Run();

            }
            catch (Exception ex)
            {
                Application.MessageBox.ShowError(IntPtr.Zero, ex.Message);
            }
        }
    }
}