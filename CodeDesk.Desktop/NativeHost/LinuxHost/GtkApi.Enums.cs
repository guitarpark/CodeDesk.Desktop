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
        internal enum GdkInterpType
        {
            GDK_INTERP_NEAREST,
            GDK_INTERP_TILES,
            GDK_INTERP_BILINEAR,
            GDK_INTERP_HYPER,
            GDK_INTERP_LAST
        }
        internal enum CairoFormat
        {
            Invalid = 0,
            ARGB32 = 1,
            RGB24 = 2,
            A8 = 3,
            A1 = 4,
            RGB16_565 = 5,
            RGB30 = 6
        }
        [Flags]
        internal enum GtkWindowType
        {
            GtkWindowToplevel=0,
            GtkWindowPopup=1
        }
        [Flags]
        internal enum GtkWindowPosition
        {
            GtkWinPosNone,
            GtkWinPosCenter,
            GtkWinPosMouse,
            GtkWinPosCenterAlways,
            GtkWinPosCenterOnParent
        }
        [Flags]
        internal enum GdkEventMask
        {
            ButtonPressMask = 1 << 2,
            ButtonReleaseMask = 1 << 3,
            PointerMotionMask = 1 << 6,
        }
        internal enum GdkEventType
        {
            MotionNotify = 6,
        }


        [Flags]
        public enum GConnectFlags
        {
            GConnectAfter,
            GConnectSwapped
        }
        public enum EventType
        {
            Nothing = -1,
            Delete = 0,
            Destroy = 1,
            Expose = 2,
            MotionNotify = 6,
            ButtonPress = 4,
            ButtonRelease = 5,
            KeyPress = 9,
            KeyRelease = 10,
            EnterNotify = 7,
            LeaveNotify = 8,
            FocusChange = 12,
            Configure = 22,
            PropertyNotify = 28,
            SelectionClear = 29,
            SelectionRequest = 30,
            SelectionNotify = 31,
            ProximityIn = 34,
            ProximityOut = 35,
            DragEnter = 36,
            DragLeave = 37,
            DragMotion = 38,
            DragStatus = 39,
            DropStart = 40,
            DropFinished = 41,
            ClientEvent = 43,
            VisibilityNotify = 44,
            Scroll = 45,
            WindowState = 47,
            Setting = 51,
            OwnerChange = 57,
            GrabBroken = 58,
            Damage = 60,
            TouchBegin = 65,
            TouchUpdate = 66,
            TouchEnd = 67,
            TouchCancel = 68,
            TouchpadSwipe = 69,
            TouchpadPinch = 70
        }

        [Flags]
        public enum GdkModifierType
        {
            None = 0,
            Shift = 1,
            Lock = 2,
            Control = 4,
            Mod1 = 8,
            Mod2 = 16,
            Mod3 = 32,
            Mod4 = 64,
            Mod5 = 128,
            Button1Mask = 256,
            Button2Mask = 512,
            Button3Mask = 1024,
            Button4Mask = 2048,
            Button5Mask = 4096,
            ModifiersMask = 255,
            ButtonMask = 65280,
            ModifierIntentMask = 16711680
        }

        public enum GdkAxisUse
        {
            Ignore,
            X,
            Y,
            Pressure,
            Xtilt,
            Ytilt,
            Wheel,
            Last
        }
        public enum CursorType
        {
            XCursor = 0,
            Arrow = 2,
            BasedArrowDown = 4,
            BasedArrowUp = 6,
            Boat = 8,
            Bogosity = 10,
            BottomLeftCorner = 12,
            BottomRightCorner = 14,
            BottomSide = 16,
            BottomTee = 18,
            BoxSpiral = 20,
            CenterPtr = 22,
            Circle = 24,
            Clock = 26,
            CoffeeMug = 28,
            Cross = 30,
            CrossReverse = 32,
            Crosshair = 34,
            DiamondCross = 36,
            Dot = 38,
            Dotbox = 40,
            DoubleArrow = 42,
            DraftLarge = 44,
            DraftSmall = 46,
            DrapedBox = 48,
            Exchange = 50,
            Fleur = 52,
            Gobbler = 54,
            Gumby = 56,
            Hand1 = 58,
            Hand2 = 60,
            Heart = 62,
            Icon = 64,
            IronCross = 66,
            LeftPtr = 68,
            LeftSide = 70,
            LeftTee = 72,
            Leftbutton = 74,
            LlAngle = 76,
            LrAngle = 78,
            Man = 80,
            Middlebutton = 82,
            Mouse = 84,
            Pencil = 86,
            Pirate = 88,
            Plus = 90,
            QuestionArrow = 92,
            RightPtr = 94,
            RightSide = 96,
            RightTee = 98,
            Rightbutton = 100,
            RtlLogo = 102,
            Sailboat = 104,
            SbDownArrow = 106,
            SbHDoubleArrow = 108,
            SbLeftArrow = 110,
            SbRightArrow = 112,
            SbUpArrow = 114,
            SbVDoubleArrow = 116,
            Shuttle = 118,
            Sizing = 120,
            Spider = 122,
            Spraycan = 124,
            Star = 126,
            Target = 128,
            Tcross = 130,
            TopLeftArrow = 132,
            TopLeftCorner = 134,
            TopRightCorner = 136,
            TopSide = 138,
            TopTee = 140,
            Trek = 142,
            UlAngle = 144,
            Umbrella = 146,
            UrAngle = 148,
            Watch = 150,
            Xterm = 152,
        }

        public enum GdkWindowEdge
        {
            None=-1,
            TopLeft = 0,
            Top = 1,
            TopRight = 2,
            Left = 3,
            Right = 4,
            BottomLeft = 5,
            Bottom = 6,
            BottomRight = 7,
        }
        public enum GdkWindowHints
        {
            GDK_HINT_MIN_SIZE = 1 << 0,
            GDK_HINT_MAX_SIZE = 1 << 1
        }
    }
}