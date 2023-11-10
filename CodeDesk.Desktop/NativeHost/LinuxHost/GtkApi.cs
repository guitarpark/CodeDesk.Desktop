using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.NativeHost.LinuxHost
{

    internal static partial class GtkApi
    {
        internal const string GtkLibraries = "libgtk-3.so.0";
        internal const string GdkLibraries = "libgdk-3.so.0";
        internal const string Gioibraries = "libgio-2.0.so.0";
        
        internal const string GObjLbraries= "libgobject-2.0.so.0";

        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_init(ref int argc, ref IntPtr argv);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void XInitThreads();
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gtk_window_new(int type);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_show(IntPtr widget);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_main();
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_main_quit();
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_destroy(IntPtr widget);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_set_default_size(IntPtr window, int width, int height);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_set_position(IntPtr window, int position);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal  static extern void gtk_window_move(IntPtr raw, int x, int y);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_set_icon(IntPtr window, IntPtr icon);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_set_decorated(IntPtr window, bool decorated);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_maximize(IntPtr window);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_iconify(IntPtr window);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_set_background(IntPtr window, IntPtr pixbuf);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gtk_widget_get_window(IntPtr widget);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)] 
        internal static extern IntPtr gtk_image_new_from_pixbuf(IntPtr pixbuf);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_container_add(IntPtr container, IntPtr widget);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_show_all(IntPtr widget);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_add_events(IntPtr widget, int events);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_get_allocation(IntPtr widget, out GdkRectangle allocation);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_window_begin_resize_drag(IntPtr window, int edge, int button, int root_x, int root_y, uint timestamp);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_cairo_create(IntPtr drawable);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cairo_rectangle(IntPtr cr, double x, double y, double width, double height);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cairo_set_source_rgb(IntPtr cr, double red, double green, double blue);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void cairo_fill(IntPtr cr);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_set_app_paintable(IntPtr widget, bool appPaintable);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_set_title(IntPtr window, string title);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_set_resizable(IntPtr window, bool resizable);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_unmaximize(IntPtr window);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_widget_override_background_color(IntPtr widget, int state,IntPtr color);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gtk_message_dialog_new(IntPtr parent, int flags, int type, int buttons,string message);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int gtk_dialog_run(IntPtr dialog);
        [DllImport(GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gtk_window_set_geometry_hints(IntPtr window, IntPtr geometry_widget, ref GdkGeometry geometry, GdkWindowHints geom_mask);
        

        
        [DllImport(GdkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_pixbuf_new_from_stream(IntPtr stream, IntPtr cancellable, IntPtr error);
        [DllImport(GdkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_pixbuf_scale_simple(IntPtr pixbuf, int width, int height,GdkInterpType interp_type);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate bool GdkEventHandler(IntPtr window, IntPtr ev, IntPtr data);
        [DllImport(GdkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gdk_window_move(IntPtr window, int x, int y);
        [DllImport(GdkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gdk_window_get_origin(IntPtr window, out int x, out int y);
        [DllImport(GdkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr gdk_cursor_new(CursorType cursor_type);
        [DllImport(GdkLibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void gdk_window_set_cursor(IntPtr window, IntPtr cursor);
        [DllImport(GdkLibraries, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint gdk_threads_add_idle(IntPtr function, IntPtr data);


        [DllImport(Gioibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr g_memory_input_stream_new_from_data(IntPtr data, long len, IntPtr destroy);

        [DllImport(Gioibraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void g_input_stream_close(IntPtr stream, IntPtr cancellable, IntPtr error);
        
        [DllImport(GObjLbraries, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint g_signal_connect_data(IntPtr instance, string detailedSignal, IntPtr handler, IntPtr data, IntPtr destroyData, uint connectFlags);


        
        internal static void SetCursor(IntPtr window, CursorType cursorType)
        {
            IntPtr cursor = gdk_cursor_new(cursorType);
            gdk_window_set_cursor(window, cursor);
        }
    }
}