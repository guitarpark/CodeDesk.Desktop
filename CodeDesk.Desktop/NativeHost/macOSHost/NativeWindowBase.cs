using System.Drawing;
using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.macOSHost
{
    internal class NativeWindowBase
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

        public virtual void Create()
        {
            Handle = macOSApi.Create(Left, Top, Size.Width, Size.Height, Chromeless,StartupCenter,CanReSize );
            macOSApi.SetTitle(Handle,Application.AppName.TomacOSIntptr());
            macOSApi.SetBackgroundColor(Handle,Application.BackgroundColor.TomacOSIntptr());
            
            if(MaximumSize.HasValue)
                macOSApi.SetMaxSize(Handle,MaximumSize.Value.Width,MaximumSize.Value.Height);
            
            if(MinimumSize.HasValue)
                macOSApi.SetMaxSize(Handle,MinimumSize.Value.Width,MinimumSize.Value.Height);
            
            var icon = Application.FileProvider.GetFileInfo(Application.Icon).CreateReadStream().StreamToByteIntptr();
            
            macOSApi.SetIcon(Handle,icon.intptr,icon.length);

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
                    break;
            }

            macOSApi.Show(Handle);
            RunMessageLoop();
        }
        internal void RunMessageLoop()
        {
            macOSApi.RunMessageLoop();
        }

        public virtual void Close()
        {
            macOSApi.Close(Handle);
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
               // var waitInfo = new GtkApi.InvokeWaitInfo() { callback = workItem };
              // GtkApi.gdk_threads_add_idle(Marshal.GetFunctionPointerForDelegate(new LinuxExtensions.InvokeCallbackWrapperDelegate(LinuxExtensions.InvokeCallback)), 
               //     GCHandle.ToIntPtr(GCHandle.Alloc(waitInfo)));
            }
            return Task.CompletedTask;
        }
        
        public async Task InvokeAsync(Func<Task> workItem)
        {
            if (Environment.CurrentManagedThreadId == managedThreadId)
                await workItem();
            else
            {
               // var waitInfo = new GtkApi.InvokeWaitInfoTask() { callback = workItem };
                
              //  GtkApi.gdk_threads_add_idle(Marshal.GetFunctionPointerForDelegate(new LinuxExtensions.InvokeCallbackWrapperDelegate(LinuxExtensions.InvokeCallback)), 
               //     GCHandle.ToIntPtr(GCHandle.Alloc(waitInfo)));
            }
        }

        public virtual void Maximize()
        {
            //GtkApi.gtk_window_maximize(Handle);
        }

        public virtual void Restore()
        {
            //GtkApi.gtk_window_unmaximize(Handle);
        }

        public virtual void Minimize()
        {
            //GtkApi.gtk_window_iconify(Handle);
        }

        public virtual void Drag(int left, int top)
        {
            //IntPtr window = GtkApi.gtk_widget_get_window(Handle);
           // GtkApi.gdk_window_get_origin(window, out int windowX, out int windowY);
           // GtkApi.gtk_window_move(Handle, windowX + left, windowY + top);
        }

        internal void ExitApplication()
        {
           // GtkApi.gtk_main_quit();
            Environment.Exit(0);
        }
    }
}