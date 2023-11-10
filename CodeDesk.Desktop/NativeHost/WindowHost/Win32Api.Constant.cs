using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.NativeHost.WindowHost
{
    internal static partial class Win32Api
    {
        internal static class CursorResource
        {
            public const int IDC_ARROW = 32512;
            public const int IDC_IBEAM = 32513;
            public const int IDC_WAIT = 32514;
            public const int IDC_CROSS = 32515;
            public const int IDC_SIZEALL = 32646;
            public const int IDC_SIZENWSE = 32642;
            public const int IDC_SIZENESW = 32643;
            public const int IDC_SIZEWE = 32644;
            public const int IDC_SIZENS = 32645;
            public const int IDC_UPARROW = 32516;
            public const int IDC_NO = 32648;
            public const int IDC_HAND = 32649;
            public const int IDC_APPSTARTING = 32650;
            public const int IDC_HELP = 32651;
        }
        internal static readonly HT[] BorderHitTestResults =
        {
            HT.TOP,
            HT.TOPLEFT,
            HT.TOPRIGHT,
            HT.BOTTOM,
            HT.BOTTOMLEFT,
            HT.BOTTOMRIGHT,
            HT.LEFT,
            HT.RIGHT,
            HT.BORDER
        };
    }
}
