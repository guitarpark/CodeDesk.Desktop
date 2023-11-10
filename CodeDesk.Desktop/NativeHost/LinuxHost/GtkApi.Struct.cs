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
        [StructLayout(LayoutKind.Sequential)]
        internal struct GdkEvent
        {
            public GdkEventType Type;
            public GdkEventMotion Motion;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdkEventMotion
        {
            public EventType type;
            public IntPtr window;
            public int send_event;
            public uint Time;
            public double X;
            public double Y;
            public double Axes;
            public IntPtr state;
            public bool is_hint;
            public IntPtr device;
            public double XRoot;
            public double YRoot;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct GdkRectangle
        {
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdkRGBA
        {
            public double red;
            public double green;
            public double blue;
            public double alpha;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdkEventButton
        {
            public EventType type;
            public IntPtr window;
            public int send_event;
            public uint time;
            public double x;
            public double y;
            public double pressure;
            public int x_root;
            public int y_root;
            public IntPtr state;
            public uint button;
            public IntPtr device;
            public double x_axis;
            public double y_axis;
            public IntPtr state_list;
            public uint scroll_direction;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GdkGeometry
        {
            public int min_width { get; set; }
            public int min_height { get; set; }
            public int max_width { get; set; }
            public int max_height { get; set; }
        }
        
        internal class InvokeWaitInfo
        {
            public Action callback;
        }
        internal class InvokeWaitInfoTask
        {
            public Func<Task> callback;
        }
    }
}