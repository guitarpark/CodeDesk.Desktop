using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.NativeHost.WindowHost
{
    internal static partial class Win32Api
    {
        internal const string User32Libraries = "user32.dll";
        internal const string Kernel32Libraries = "kernel32.dll";
        internal const string Gdi32Libraries = "gdi32.dll";
        internal const string Comdlg32Libraries = "comdlg32.dll";
        internal const string shell32Libraries = "shell32.dll";
    }
}
