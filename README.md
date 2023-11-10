###  **多平台支持 ** 

 
CodeDesk帮助开发者创建适用于主要桌面平台的应用程序。它提供了跨平台的能力，使开发者能够使用几乎所有现有的前端框架来构建应用程序。无论是Html,Blazor,Vue,React还是Angular等前端框架，都可以与CodeDesk一起使用。这种多平台支持使得开发者能够在不同的桌面操作系统上快速构建和维护应用程序。 
 
###  **基于.NETCORE支持： ** 

 
CodeDesk基于.NetCore开发。可以使用现有丰富的.NetCore的组件库，简化大量的基础组件。具有很好的可扩展性，支持多种扩展方式，如中间件、过滤器、插件等，可以非常方便开发者扩展应用程序的功能。具有一系列的安全优点，可以帮助开发人员构建更加安全和可靠的应用程序，避免常见的安全问题和漏洞，如：身份验证、授权、输入验证、防止跨站点脚本攻击等。 
 
###  **体积小、高性能 ** 

 
CodeDesk安装包体积小，相比其他安装包动辄上百兆，使用CodeDesk开发的应用安装包最小可以做到不到10MB。使用操作系统内置的基于Chromium或WebKit的浏览器控件，适用于Windows、macOS和Linux。利用Blazor的热更新，使得开发效率大幅度提高，无需繁琐的调试即可看到最终的呈现效果。 
 
###  **开源 ** 

 
CodeDesk是一个开源项目，它依赖于社区的参与和贡献。开源意味着任何人都可以查看、使用和修改源代码。这种透明性和开放性有助于社区合作和创造力的发挥。CodeDesk的开源性也使得它可以被认证为“真正”的开源应用程序。 
 
###  **CodeDesk 入门**  

 
要开始使用，只需按照以下说明操作即可。有适用于Blazor、Html的示例。我们在示例应用中添加了一些有用的注释来解释所有内容。


```
try  
{  
    AppDomain.CurrentDomain.UnhandledException += (s, e) =>  
    {  
        Application.MessageBox.ShowError(IntPtr.Zero, e.ToString());  
    };  
    var builder = Application.Initialize();  
    Application.MessageReceivedHandler = new Action<string>((message) =>  
    {   
        System.Diagnostics.Debug.WriteLine("customMessage" + message);  
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
        WebAppType = WebAppType.Blazor,//还可以设置Local,Http，同时需要修改url入口地址  
        Url = "http://localhost/blazorindex.html" //local_left_index.html,local_top_index.html    
        BlazorComponent = typeof(App),//blazor必须设置   
        BlazorSelector = "#app",//blazor必须设置    
    };   
    builder.CreateWindow(splashConfig, windowConfig);    
    builder.Run();   
}    
catch (Exception ex)   
{    
    Application.MessageBox.ShowError(IntPtr.Zero, ex.Message);  
}   
```

###  **适用于.NET和Blazor的CodeDesk ** 

 
使用CodeDesk您可以使用.NET后端和您选择的Web框架构建桌面应用程序。由于.NET是跨平台的，因此它是在任何地方运行的可靠且可重用的代码的完美候选者。CodeDesk的Blazor功能，并添加了无需了解JavaScript或TypeScript即可生成应用程序的功能。同时由于没有Electron的NodeJs，所以它是一个干净的，快速的桌面端程序。 
 
###  **关于CodeDesk ** 

 
CodeDesk的灵感来自Electron和Photino。这是一个基于.NET的开源项目。CodeDesk的目标是使开发人员能够在跨平台的本机应用程序中使用Web UI（HTML、JavaScript、CSS等），而不是学习特定于平台的UI技术。 
 
###  **CodeDesk是如何工作的？ ** 

 
CodeDesk是可用于不同平台和技术的软件包集合。围绕着操作系统内置的Chromium或基于WebKit的浏览器控件。在Windows上，它使用基于Chromium Edge的WebView2控件，在macOS上，它使用Safari的WKWebView，在Linux上，它使用WebKitGTK+2。使用本机浏览器控件可以减少CodeDesk应用程序的整体占用空间，因为无需在应用程序中捆绑WebKit。CodeDesk是跨平台的，因为应用程序在任何操作系统上都以完全相同的方式与包装器通信。这意味着开发人员只需要一个代码库即可应用于所有桌面平台。 
 
###  **QQ群：882080474 
网址：http://www.codedesk.cn/** 
