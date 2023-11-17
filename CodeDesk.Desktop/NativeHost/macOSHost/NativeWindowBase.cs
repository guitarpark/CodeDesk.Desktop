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
            
            // var windowType = Chromeless ? GtkApi.GtkWindowType.GtkWindowPopup : GtkApi.GtkWindowType.GtkWindowToplevel;
            // Handle = GtkApi.gtk_window_new((int)windowType);
            // GtkApi.gtk_window_set_title(Handle, Application.AppName);
            // GtkApi.gtk_window_set_resizable(Handle, true);
            //
            // var iconPtr = Application.FileProvider.GetFileInfo(Application.Icon).CreateReadStream().StreamToByte()
            //     .GetPixbuf();
            // GtkApi.gtk_window_set_icon(Handle,
            //     GtkApi.gdk_pixbuf_scale_simple(iconPtr, 64, 64, GtkApi.GdkInterpType.GDK_INTERP_BILINEAR));
            // if (Chromeless)
            //     GtkApi.gtk_window_set_decorated(Handle, true);
            //
            // GtkApi.gtk_window_set_default_size(Handle, Size.Width, Size.Height);
            // //GtkApi.gtk_widget_set_app_paintable(Handle, true);
            //
            // GtkApi.gtk_widget_override_background_color(Handle, 0, Application.BackgroundColor.ToGdkRGBAIntPtr());
            //
            // if (StartupCenter)
            // {
            //     GtkApi.gtk_window_set_position(Handle, (int)GtkApi.GtkWindowPosition.GtkWinPosCenter);
            // }
            // else
            // {
            //     GtkApi.gtk_window_move(Handle, Left, Top);
            // }
            //
            // var geometry = new GtkApi.GdkGeometry()
            // {
            //     min_width = MinimumSize.HasValue?MinimumSize.Value.Width:0,
            //     min_height = MinimumSize.HasValue?MinimumSize.Value.Height:0,
            //     max_width = MaximumSize.HasValue?MaximumSize.Value.Width:0,
            //     max_height = MaximumSize.HasValue?MaximumSize.Value.Height:0
            // };
            // GtkApi.gtk_window_set_geometry_hints(Handle, IntPtr.Zero, ref geometry,
            //     GtkApi.GdkWindowHints.GDK_HINT_MIN_SIZE | GtkApi.GdkWindowHints.GDK_HINT_MAX_SIZE);
            //
            // //注册事件
            // GtkApi.gtk_widget_add_events(Handle,
            //     (int)(GtkApi.GdkEventMask.PointerMotionMask) |
            //     (int)(GtkApi.GdkEventMask.ButtonPressMask | GtkApi.GdkEventMask.ButtonReleaseMask));
            // RegisterHandel(Handle, "button-press-event", new GtkWidgetEventDelegate(OnButtonPressEvent));
            // RegisterHandel(Handle, "button-release-event", new GtkWidgetEventDelegate(OnButtonReleaseEvent));
            // RegisterHandel(Handle, "motion-notify-event", new GtkWidgetEventDelegate(OnMotionNotifyEvent));
            // RegisterHandel(Handle, "leave-notify-event", new GtkWidgetEventDelegate(OnLeaveNotifyEvent));
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
            //GtkApi.gtk_widget_destroy(Handle);
           // GtkApi.gtk_main_quit();
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