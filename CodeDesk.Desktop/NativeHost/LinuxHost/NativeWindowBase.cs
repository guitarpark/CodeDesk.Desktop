using System.Drawing;
using System.Runtime.InteropServices;
using CodeDesk.Desktop.WebViewHost.GtkWebkitHost;

namespace CodeDesk.Desktop.NativeHost.LinuxHost
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
            var windowType = Chromeless ? GtkApi.GtkWindowType.GtkWindowPopup : GtkApi.GtkWindowType.GtkWindowToplevel;
            Handle = GtkApi.gtk_window_new((int)windowType);
            GtkApi.gtk_window_set_title(Handle, Application.AppName);
            GtkApi.gtk_window_set_resizable(Handle, true);

            var iconPtr = Application.FileProvider.GetFileInfo(Application.Icon).CreateReadStream().StreamToByte()
                .GetPixbuf();
            GtkApi.gtk_window_set_icon(Handle,
                GtkApi.gdk_pixbuf_scale_simple(iconPtr, 64, 64, GtkApi.GdkInterpType.GDK_INTERP_BILINEAR));
            if (Chromeless)
                GtkApi.gtk_window_set_decorated(Handle, true);

            GtkApi.gtk_window_set_default_size(Handle, Size.Width, Size.Height);
            //GtkApi.gtk_widget_set_app_paintable(Handle, true);

            GtkApi.gtk_widget_override_background_color(Handle, 0, Application.BackgroundColor.ToGdkRGBAIntPtr());

            if (StartupCenter)
            {
                GtkApi.gtk_window_set_position(Handle, (int)GtkApi.GtkWindowPosition.GtkWinPosCenter);
            }
            else
            {
                GtkApi.gtk_window_move(Handle, Left, Top);
            }

            var geometry = new GtkApi.GdkGeometry()
            {
                min_width = MinimumSize.HasValue?MinimumSize.Value.Width:0,
                min_height = MinimumSize.HasValue?MinimumSize.Value.Height:0,
                max_width = MaximumSize.HasValue?MaximumSize.Value.Width:0,
                max_height = MaximumSize.HasValue?MaximumSize.Value.Height:0
            };
            GtkApi.gtk_window_set_geometry_hints(Handle, IntPtr.Zero, ref geometry,
                GtkApi.GdkWindowHints.GDK_HINT_MIN_SIZE | GtkApi.GdkWindowHints.GDK_HINT_MAX_SIZE);

            //注册事件
            GtkApi.gtk_widget_add_events(Handle,
                (int)(GtkApi.GdkEventMask.PointerMotionMask) |
                (int)(GtkApi.GdkEventMask.ButtonPressMask | GtkApi.GdkEventMask.ButtonReleaseMask));
            RegisterHandel(Handle, "button-press-event", new GtkWidgetEventDelegate(OnButtonPressEvent));
            RegisterHandel(Handle, "button-release-event", new GtkWidgetEventDelegate(OnButtonReleaseEvent));
            RegisterHandel(Handle, "motion-notify-event", new GtkWidgetEventDelegate(OnMotionNotifyEvent));
            RegisterHandel(Handle, "leave-notify-event", new GtkWidgetEventDelegate(OnLeaveNotifyEvent));
        }

        private Point drag;
        private bool isDrag = false;
        private GtkApi.GdkWindowEdge windowEdge = GtkApi.GdkWindowEdge.None;

        protected delegate bool GtkWidgetEventDelegate(IntPtr widget, IntPtr ev, IntPtr data);

        protected virtual bool OnLeaveNotifyEvent(IntPtr widget, IntPtr ev, IntPtr data)
        {
            isDrag = false;
            return false;
        }

        protected virtual bool OnMotionNotifyEvent(IntPtr widget, IntPtr ev, IntPtr data)
        {
            var eventMotion = Marshal.PtrToStructure<GtkApi.GdkEventMotion>(ev);
            IntPtr window = GtkApi.gtk_widget_get_window(widget);

            // 获取窗体的边界
            GtkApi.gtk_widget_get_allocation(widget, out GtkApi.GdkRectangle allocation);
            if (isDrag)
            {
                GtkApi.gdk_window_get_origin(window, out int windowX, out int windowY);
                switch (windowEdge)
                {
                    case GtkApi.GdkWindowEdge.None:
                    {
                        int deltaX = (int)eventMotion.X - drag.X;
                        int deltaY = (int)eventMotion.Y - drag.Y;
                        GtkApi.gdk_window_move(window, windowX + deltaX, windowY + deltaY);
                        break;
                    }
                    case GtkApi.GdkWindowEdge.Bottom:
                    {
                        GtkApi.gdk_window_begin_resize_drag(window, (int)windowEdge, 1, windowX,
                            windowY + allocation.Height, eventMotion.Time);
                        break;
                    }
                    case GtkApi.GdkWindowEdge.BottomRight:
                    {
                        GtkApi.gdk_window_begin_resize_drag(window, (int)windowEdge, 1, windowX + allocation.Width,
                            windowY + allocation.Height, eventMotion.Time);
                        break;
                    }
                    case GtkApi.GdkWindowEdge.BottomLeft:
                    {
                        GtkApi.gdk_window_begin_resize_drag(window, (int)windowEdge, 1, windowX,
                            windowY + allocation.Height, eventMotion.Time);
                        break;
                    }
                    case GtkApi.GdkWindowEdge.Right:
                    case GtkApi.GdkWindowEdge.TopRight:
                    {
                        GtkApi.gdk_window_begin_resize_drag(window, (int)windowEdge, 1, windowX + allocation.Width,
                            windowY, eventMotion.Time);
                        break;
                    }
                    default:
                    {
                        GtkApi.gdk_window_begin_resize_drag(window, (int)windowEdge, 1, windowX, windowY,
                            eventMotion.Time);
                        break;
                    }
                }
            }
            else
            {
                if (!CanReSize)
                    return false;
                var left = System.Math.Abs(eventMotion.X - allocation.X) <= 10;
                var top = System.Math.Abs(eventMotion.Y - allocation.Y) <= 10;
                var right = System.Math.Abs(eventMotion.X - allocation.Width) <= 10;
                var bottom = System.Math.Abs(eventMotion.Y - allocation.Height) <= 10;

                if (left && top)
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.TopLeftCorner);
                    windowEdge = GtkApi.GdkWindowEdge.TopLeft;
                }
                else if (top && right)
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.TopRightCorner);
                    windowEdge = GtkApi.GdkWindowEdge.TopRight;
                }
                else if (right && bottom)
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.BottomRightCorner);
                    windowEdge = GtkApi.GdkWindowEdge.BottomRight;
                }
                else if (bottom && left)
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.BottomLeftCorner);
                    windowEdge = GtkApi.GdkWindowEdge.BottomLeft;
                }
                else if (left)
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.LeftSide);
                    windowEdge = GtkApi.GdkWindowEdge.Left;
                }
                else if (top)
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.TopSide);
                    windowEdge = GtkApi.GdkWindowEdge.Top;
                }
                else if (right)
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.RightSide);
                    windowEdge = GtkApi.GdkWindowEdge.Right;
                }
                else if (bottom)
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.BottomSide);
                    windowEdge = GtkApi.GdkWindowEdge.Bottom;
                }
                else
                {
                    GtkApi.SetCursor(window, GtkApi.CursorType.Arrow);
                    windowEdge = GtkApi.GdkWindowEdge.None;
                }
            }

            return false;
        }

        protected virtual bool OnButtonReleaseEvent(IntPtr widget, IntPtr ev, IntPtr data)
        {
            System.Diagnostics.Debug.WriteLine("window-OnButtonReleaseEvent");
            IntPtr window = GtkApi.gtk_widget_get_window(widget);
            GtkApi.SetCursor(window, GtkApi.CursorType.Arrow);
            drag = new Point(0, 0);
            isDrag = false;
            return false;
        }

        protected virtual bool OnButtonPressEvent(IntPtr widget, IntPtr ev, IntPtr data)
        {
            isDrag = true;
            var eventButton = Marshal.PtrToStructure<GtkApi.GdkEventMotion>(ev);
            drag.X = (int)eventButton.X;
            drag.Y = (int)eventButton.Y;
            return false;
        }

        protected void RegisterHandel(IntPtr handle, string name, GtkWidgetEventDelegate @event)
        {
            GtkApi.g_signal_connect_data(handle, name,
                Marshal.GetFunctionPointerForDelegate<GtkWidgetEventDelegate>(@event), IntPtr.Zero, IntPtr.Zero, 0);
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

            GtkApi.gtk_widget_show_all(Handle);
        }

        internal void RunMessageLoop()
        {
            GtkApi.gtk_main();
        }

        public virtual void Close()
        {
            GtkApi.gtk_widget_destroy(Handle);
            GtkApi.gtk_main_quit();
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
                var waitInfo = new GtkApi.InvokeWaitInfo() { callback = workItem };
               GtkApi.gdk_threads_add_idle(Marshal.GetFunctionPointerForDelegate(new LinuxExtensions.InvokeCallbackWrapperDelegate(LinuxExtensions.InvokeCallback)), 
                    GCHandle.ToIntPtr(GCHandle.Alloc(waitInfo)));
            }
            return Task.CompletedTask;
        }
        
        public async Task InvokeAsync(Func<Task> workItem)
        {
            if (Environment.CurrentManagedThreadId == managedThreadId)
                await workItem();
            else
            {
                var waitInfo = new GtkApi.InvokeWaitInfoTask() { callback = workItem };
                
                GtkApi.gdk_threads_add_idle(Marshal.GetFunctionPointerForDelegate(new LinuxExtensions.InvokeCallbackWrapperDelegate(LinuxExtensions.InvokeCallback)), 
                    GCHandle.ToIntPtr(GCHandle.Alloc(waitInfo)));
            }
        }

        public virtual void Maximize()
        {
            GtkApi.gtk_window_maximize(Handle);
        }

        public virtual void Restore()
        {
            GtkApi.gtk_window_unmaximize(Handle);
        }

        public virtual void Minimize()
        {
            GtkApi.gtk_window_iconify(Handle);
        }

        public virtual void Drag(int left, int top)
        {
            IntPtr window = GtkApi.gtk_widget_get_window(Handle);
            GtkApi.gdk_window_get_origin(window, out int windowX, out int windowY);
            GtkApi.gtk_window_move(Handle, windowX + left, windowY + top);
        }

        internal void ExitApplication()
        {
            GtkApi.gtk_main_quit();
            Environment.Exit(0);
        }
    }
}