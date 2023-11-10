using CodeDesk.Desktop.Browser;
using CodeDesk.Desktop.NativeHosts.WindowHost;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Drawing;
using System.Net;
using System.Reflection;
using Xilium.CefGlue;

namespace CodeDesk.Desktop
{
    public partial class AppBuilder
    {
        internal static void CefInitializeRuntime()
        {
            var cefArgs = new CefMainArgs(null);
            var cefApp = new CodeDeskBrowserApp();

            CefRuntime.Load();
            CefRuntime.ExecuteProcess(cefArgs, cefApp, IntPtr.Zero);
            var settings = new CefSettings
            {
                WindowlessRenderingEnabled = true,
                MultiThreadedMessageLoop = true,
                LogSeverity = CefLogSeverity.Verbose,
                LogFile = "cef.log",

            };
            CefRuntime.Initialize(cefArgs, settings, cefApp);
           // CefRuntime.RunMessageLoop();
        }
    }
}
