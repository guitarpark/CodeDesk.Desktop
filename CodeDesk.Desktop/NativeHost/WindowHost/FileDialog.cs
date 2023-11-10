using System.Runtime.InteropServices;
using System.Linq;

namespace CodeDesk.Desktop.NativeHost.WindowHost
{
    internal class FileDialog : IFileDialog
    {
        const int bufferLength = 2048;
        /// <summary>
        /// 返回选中文件
        /// </summary>
        /// <param name="initialDir"></param>
        /// <param name="filters"></param>
        /// <returns>fileName：文件名，fileFullName：包含路径的文件名</returns>
        public (bool Selected, string fileName, string fileFullName) OpenFile(string initialDir = null, Dictionary<string, string> filters = null)
        {
            if (filters == null)
            {
                filters = new Dictionary<string, string>() { { "所有文件（*.*)", "*.*" } };
            }
            if (string.IsNullOrEmpty(initialDir))
            {
                initialDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            OpenFileDialogParams @params = new OpenFileDialogParams()
            {
                ownerHandle = IntPtr.Zero,
                instanceHandle = IntPtr.Zero,
                filter = string.Join("", filters.Select(s => $"{s.Key}\0{s.Value}\0")),
                initialDir = initialDir,
                file = Marshal.StringToBSTR(new string(new char[bufferLength])),
                maxFile = bufferLength,
                maxFileTitle = bufferLength,
                fileTitle = new string(new char[bufferLength]),
                title = "打开文件",
                flags = 0x00080000 | 0x00001000
            };
            @params.structSize = Marshal.SizeOf(@params);
            if (Win32Api.GetOpenFileName(ref @params))
            {
                string file = Marshal.PtrToStringAuto(@params.file);
                return (true, @params.fileTitle, file);
            }
            return (false, "", "");
        }
        /// <summary>
        /// 返回选中文件列表
        /// </summary>
        /// <param name="initialDir"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public (bool Selected, Dictionary<string, string> Files) OpenFiles(string initialDir = null, Dictionary<string, string> filters = null)
        {

            if (filters == null)
            {
                filters = new Dictionary<string, string>() { { "所有文件（*.*)", "*.*" } };
            }
            if (string.IsNullOrEmpty(initialDir))
            {
                initialDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            OpenFileDialogParams @params = new OpenFileDialogParams()
            {
                ownerHandle = IntPtr.Zero,
                instanceHandle = IntPtr.Zero,
                filter = string.Join("", filters.Select(s => $"{s.Key}\0{s.Value}\0")),
                initialDir = initialDir,
                file = Marshal.StringToBSTR(new String(new char[bufferLength])),
                maxFile = bufferLength,
                fileTitle = new string(new char[bufferLength]),
                title = "打开文件",
                flags = 0x00000004 | 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008 | 0x00000200
            };
            @params.maxFileTitle = @params.fileTitle.Length;
            @params.structSize = Marshal.SizeOf(@params);

            if (Win32Api.GetOpenFileName(ref @params))
            {
                Dictionary<string, string> selectedFiles = new Dictionary<string, string>();

                long pointer = (long)@params.file;
                string file = Marshal.PtrToStringAuto(@params.file);

                var path = "";
                var index = 0;

                while (file.Length > 0)
                {
                    if (index == 0)
                    {
                        path = file;
                    }
                    else
                    {
                        selectedFiles.Add(file, System.IO.Path.Combine(path, file));
                    }

                    pointer += file.Length * 2 + 2;
                    @params.file = (IntPtr)pointer;
                    file = Marshal.PtrToStringAuto(@params.file);
                    index++;
                }
                return (true, selectedFiles);
            }
            return (false, null);
        }

        public (bool Selected, string fileName, string fileFullName) SaveFile(string fileName, string initialDir = null, Dictionary<string, string> filters = null)
        {
            if (filters == null)
            {
                filters = new Dictionary<string, string>() { { "所有文件（*.*)", "*.*" } };
            }
            if (string.IsNullOrEmpty(initialDir))
            {
                initialDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            var @params = new SAVEFILENAME()
            {
                lpstrFilter = string.Join("|", filters.Select(s => $"{s.Key}\0{s.Value}\0")),
                lpstrFile = fileName ?? new string(new char[bufferLength]),
                lpstrFileTitle = new string(new char[bufferLength]),
                lpstrInitialDir = initialDir,
                lpstrTitle = "保存文件",
                nMaxFile = bufferLength,
                nMaxFileTitle = bufferLength
            };
            @params.lStructSize = Marshal.SizeOf(@params);
            if (Win32Api.GetSaveFileName(ref @params))
            {
                return (true, @params.lpstrFileTitle, @params.lpstrFile);
            }
            return (false, "", "");
        }

        public (bool Selected, string path) OpenDirectory(string initialDir = null)
        {
            if (string.IsNullOrEmpty(initialDir))
            {
                initialDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }

            IntPtr pidl = IntPtr.Zero;
            var @params = new BROWSEINFO()
            {
                hwndOwner = IntPtr.Zero,
                pidlRoot = IntPtr.Zero,
                pszDisplayName = IntPtr.Zero,
                lpszTitle = "选择目录",
                ulFlags = 0,
                lpfn = IntPtr.Zero,
                lParam = IntPtr.Zero,
                iImage = 0
            };
            try
            {
                pidl = Win32Api.SHBrowseForFolder(ref @params);
                if (pidl != IntPtr.Zero)
                {
                    IntPtr pszPath = Marshal.AllocHGlobal(bufferLength);
                    if (Win32Api.SHGetPathFromIDList(pidl, pszPath))
                    {
                        Marshal.FreeHGlobal(pszPath);
                        return (true, Marshal.PtrToStringAuto(pszPath));
                    }
                    else
                    {
                        return (false, "当前目录太长或权限不足！");
                    }
                }
            }
            finally
            {
                if (pidl != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pidl);
                }
            }

            return (false, "");
        }
    }
}


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct OpenFileDialogParams
{
    public int structSize;
    public IntPtr ownerHandle;
    public IntPtr instanceHandle;
    public string filter;
    public string customFilter;
    public int filterIndex;
    public IntPtr file;
    public int maxFile;
    public string fileTitle;
    public int maxFileTitle;
    public string initialDir;
    public string title;
    public int flags;
    public short fileOffset;
    public short fileExtension;
    public string defExt;
    public IntPtr custData;
    public IntPtr hook;
    public string templateName;
    public IntPtr reservedPtr;
    public int reservedInt;
    public int flagsEx;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct SAVEFILENAME
{
    public int lStructSize;
    public IntPtr hwndOwner;
    public IntPtr hInstance;
    public string lpstrFilter;
    public string lpstrCustomFilter;
    public int nMaxCustFilter;
    public int nFilterIndex;
    public string lpstrFile;
    public int nMaxFile;
    public string lpstrFileTitle;
    public int nMaxFileTitle;
    public string lpstrInitialDir;
    public string lpstrTitle;
    public int Flags;
    public short nFileOffset;
    public short nFileExtension;
    public string lpstrDefExt;
    public IntPtr lCustData;
    public IntPtr lpfnHook;
    public string lpTemplateName;
    public IntPtr pvReserved;
    public int dwReserved;
    public int FlagsEx;
}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct BROWSEINFO
{
    public IntPtr hwndOwner;
    public IntPtr pidlRoot;
    public IntPtr pszDisplayName;
    public string lpszTitle;
    public uint ulFlags;
    public IntPtr lpfn;
    public IntPtr lParam;
    public int iImage;
}

//FlagsEx参数的一些常用选项及其对应的值：

//OFN_EX_NO_READONLY_WARNING：禁止显示只读警告。值为0x00000001。
//OFN_EX_NO_TEST_FILE_CREATE：禁止测试文件创建。值为0x00000002。
//OFN_EX_HIDE_READONLY：隐藏只读复选框。值为0x00000004。
//OFN_EX_HIDE_OPEN_WITH：隐藏"打开方式"选项。值为0x00000008。
//OFN_EX_HIDE_SHARE：隐藏共享选项。值为0x00000010。
//OFN_EX_HIDE_HELP：隐藏帮助按钮。值为0x00000020。
//OFN_EX_HIDE_CANCEL：隐藏取消按钮。值为0x00000040。
//OFN_EX_HIDE_ABORT：隐藏中止按钮。值为0x00000080。
//OFN_EX_NO_CUSTOMIZE：禁止自定义对话框。值为0x00000100。
//OFN_EX_NO_DEREFERENCE_LINKS：禁止跟踪快捷方式链接。值为0x00001000。
//OFN_EX_NOVALIDATE：禁止验证输入的文件名。值为0x01000000。
//OFN_EX_HIDE_FILTER：隐藏过滤器选项。值为0x20000000。
//OFN_EX_HIDE_FILE：隐藏文件名输入框。值为0x40000000。
//OFN_EX_HIDE_FILEEXT：隐藏文件扩展名输入框。值为0x80000000。
//OFN_EX_HIDE_PATH：隐藏路径输入框。值为Ox1382F674。
//OFN_EX_HIDE_DIR：隐藏目录输入框。值为Ox1382F675。
//OFN_EX_NOCLOSEONEXEC：执行应用程序时，不关闭对话框。值为Ox1382F676。
//OFN_EX_SHOWHELP：显示帮助按钮。值为Ox1382F677。