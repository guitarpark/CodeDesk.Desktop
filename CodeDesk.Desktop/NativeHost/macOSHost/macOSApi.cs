﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.NativeHost.macOSHost
{
    internal partial class macOSApi
    {
        internal const string libmacOSLibrary = "libmacOSLibrary.dylib";
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Create(int left, int top, int width, int height, bool chromeless,bool center,bool canResize);
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetTitle(IntPtr window,IntPtr title);
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetBackgroundColor(IntPtr window,IntPtr backgroundColor);
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Show(IntPtr window);
        
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetMaxSize(IntPtr window,int width,int height);
        
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetMinSize(IntPtr window,int width,int height);
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void RunMessageLoop();
        public static void LoadNativeHostFile()
        {
            string resourcePath = $"CodeDesk.Desktop.NativeHost.macOSHost.{libmacOSLibrary}";
            using var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            if (resource is not null)
            {
                using var file = new FileStream(libmacOSLibrary, FileMode.Create, FileAccess.Write);
                resource.CopyTo(file);
            }
        }
    }
}
