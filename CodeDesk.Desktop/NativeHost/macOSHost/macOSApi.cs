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
        internal static extern IntPtr Create(int left, int top, int width, int height, bool chromeless);
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetTitle(IntPtr window, string title);
        [DllImport(libmacOSLibrary, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Show(IntPtr window);
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