using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesk.Desktop.Extensions
{
    public static class UtilitiesExtensions
    {
        public static string GetApplicationPath(string file)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
        }

        /// <summary>
        /// 检查Manifests文件是否存在
        /// </summary>
        /// <param name="Manifests"></param>
        /// <returns></returns>
        public static bool CheckEmbeddedManifest(this Assembly assembly)
        {
            return assembly.GetManifestResourceNames()
                .Any(s => s.Equals("Microsoft.Extensions.FileProviders.Embedded.Manifest.xml"));
        }

        public static (double R, double G, double B,double A) HtmlColorToRgb(this string htmlColor)
        {
            var color=ColorTranslator.FromHtml(htmlColor);
            return (color.R / 255.0, color.G / 255.0, color.B / 255.0,color.A/255.0);
        }

        public static int ToInt(this object value)
        {
            if (value == null)
                return 0;
            return Convert.ToInt32(value);
        }

        public static byte[] StreamToByte(this Stream stream)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static IntPtr StructToIntPtr<T>(this T type)
        {
            IntPtr result = Marshal.AllocHGlobal(Marshal.SizeOf(type));
            Marshal.StructureToPtr(type, result, false);
            return result;
        }

        public static (IntPtr IntPtr,int Length) StreamToIntptr(this Stream stream)
        {
            var bytes = StreamToByte(stream);
            IntPtr bytesPtr = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, bytesPtr, bytes.Length);
            return (bytesPtr, bytes.Length);
        }

        public static string EscapeJson(this string str)
        {
            return str.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
        }

        public static (string output,string error) ExecuteCommand(string file, string args, Action<int> progress)
        {

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = file,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += (s, e) => { progress?.Invoke(e.Data.ToInt()); };
            process.Exited += (s, e) =>
            {
                if (process.ExitCode != 0)
                    throw new Exception("安装运行库失败！");
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return (output, error);
        }

        public static List<string> PtrToStringList(this IntPtr strs)
        {
            var result = new List<string>();
            int i = 0;
            while (true)
            {
                IntPtr filePathPtr = Marshal.ReadIntPtr(strs, i * IntPtr.Size);
                if (filePathPtr == IntPtr.Zero)
                    break;
                string filePath = Marshal.PtrToStringAnsi(filePathPtr);
                result.Add(filePath);
                i++;
            }

            return result;
        }
    }
}