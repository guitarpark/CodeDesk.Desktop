﻿using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
namespace CodeDesk.Desktop.NativeHost.macOSHost
{
    internal class NativeSplashWindow : NativeWindowBase, ISplashWindow
    {
        SplashConfig Option;
        WindowConfig WindowOption;
        internal NativeSplashWindow(SplashConfig option, WindowConfig windowOption)
        {
            macOSApi.LoadNativeHostFile();
            base.Chromeless = false;
            base.Size = option.Size;
            base.StartupCenter = true;

            this.Option = option;
            this.WindowOption = windowOption;
        }

        public override void Create()
        {
            base.Create();
           var progressIndicator= macOSApi.createProgressIndicator(Handle, 
                (Option.Loading.Left),(Option.Size.Height- Option.Loading.Top),
                Option.Loading.Width, Option.Loading.Height,
                Option.Loading.Color.TomacOSIntptr());
           ShowSplashScreen(progressIndicator);
        }
        Timer timer;
        void ShowSplashScreen(IntPtr progressIndicator)
        {
            int loadingWidth = 0;
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
                    macOSApi.setProgressBarValue(progressIndicator,loadingWidth);
                }
            }, null, 0, loadingInterval);

        }
        public override void Show()
        {
            base.Show();
        }
        public void Close()
        {
        }
    }
}