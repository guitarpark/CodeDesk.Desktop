using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace CodeDesk.Desktop
{
    public partial class Application
    {

        /// <summary>
        /// 平台信息
        /// </summary>
        public static RuntimePlatform Platform = RuntimePlatformExtensions.DetectPlatform();
        /// <summary>
        /// 是否64位
        /// </summary>
        public static bool Is64Bit => RuntimePlatformExtensions.Is64Bit();
        public static string BackgroundColor { get; set; } = "#f5f5f5";
        public static string AppName { get; set; } = "新建跨平台程序";
        public static string Icon { get; set; } = "icon-drak.png";
        public static Application Initialize()
        {
            return new Application();
        }
        public static IMessageBox MessageBox => Services.Value.GetRequiredService<IMessageBox>();
        public static IFileDialog FileDialog => Services.Value.GetRequiredService<IFileDialog>();

        public void CreateWindow(SplashConfig splashConfig = null, WindowConfig windowConfig = null)
        {
            windowConfig = windowConfig ?? new WindowConfig();
            splashConfig = splashConfig ?? new SplashConfig();

            if (windowConfig.WebAppType != WebAppType.Http)
            {
                serviceCollection.Value.AddSingleton<JSComponentConfigurationStore>();
                serviceCollection.Value.AddBlazorWebView();
            }

            SplashWindow = GetSplashWindow(splashConfig, windowConfig);
        }
        public static Action<string> MessageReceivedHandler;
        ISplashWindow SplashWindow;
        public void Run()
        {
            SplashWindow.Show();
        }
        #region serviceCollection
        static Lazy<IServiceCollection> serviceCollection = new Lazy<IServiceCollection>(() =>
        {
            var services = new ServiceCollection();
            switch (Platform)
            {
                case RuntimePlatform.Windows:
                    {
                        services.AddSingleton<IMessageBox,NativeHost.WindowHost.MessageBox>();
                        services.AddSingleton<IFileDialog, NativeHost.WindowHost.FileDialog>();
                        break;
                    }
                default:
                    {
                        services.AddSingleton<IMessageBox, NativeHost.LinuxHost.MessageBox>();
                        services.AddSingleton<IFileDialog, NativeHost.LinuxHost.FileDialog>();
                        break;
                    }
            }
            return services;
        });

        internal static Lazy<ServiceProvider> Services = new Lazy<ServiceProvider>(() =>
          {
              return serviceCollection.Value.BuildServiceProvider();
          });
        private static readonly string ContentRoot = "wwwroot";
        public Application RegisterResource(params Type[] types)
        {
            //注册启动程序
            List<IFileProvider> containers = new List<IFileProvider>();

            var entryAssembly = Assembly.GetEntryAssembly();
            containers.Add(new PhysicalFileProvider(Path.GetDirectoryName(entryAssembly.Location)));

            containers.Add(new ManifestEmbeddedFileProvider(typeof(Application).Assembly, ContentRoot));
            containers.Add(new ManifestEmbeddedFileProvider(typeof(Microsoft.AspNetCore.Components.WebView.WebViewManager).Assembly));

            //加入启动程序资源文件
            if (entryAssembly.CheckEmbeddedManifest())
            {
                containers.Add(new ManifestEmbeddedFileProvider(entryAssembly, ContentRoot));
            }
            //加入自定义资源
            types.Where(s => s.Assembly.CheckEmbeddedManifest()).ToList().ForEach(type =>
            {
                containers.Add(new ManifestEmbeddedFileProvider(type.Assembly, ContentRoot));
            });

            IFileProvider provider = new CompositeFileProvider(containers);
            serviceCollection.Value.AddSingleton(provider);
            return this;
        }
        public Application AddRootComponent(Type component, string selector)
        {
          
            return this;
        }
        #endregion
        public static IFileProvider FileProvider => Services.Value.GetService<IFileProvider>();

        private static ISplashWindow GetSplashWindow(SplashConfig startupOption,WindowConfig windowOption)
        {
            switch (Platform)
            {
                case RuntimePlatform.Windows:
                    {
                        var window = new NativeHost.WindowHost.NativeSplashWindow(startupOption, windowOption);
                        window.Create();
                        return window;
                    }
                case RuntimePlatform.Linux:
                    {
                        var window = new NativeHost.LinuxHost.NativeSplashWindow(startupOption, windowOption);
                        window.Create();
                        return window;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
