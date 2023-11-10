using System.Runtime.InteropServices;

namespace CodeDesk.Desktop.NativeHost.LinuxHost
{
    internal class FileDialog : IFileDialog
    {
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr gtk_file_chooser_dialog_new(string title, IntPtr parent, FileChooserAction action, string button1, ResponseType response1, string button2, ResponseType response2, string button3, ResponseType response3, IntPtr nullTerminated);

        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr gtk_file_chooser_get_filename(IntPtr chooser);

        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern void gtk_file_chooser_set_current_folder(IntPtr chooser, string folder);

        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern void gtk_file_chooser_set_current_name(IntPtr chooser, string name);

        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern void gtk_file_chooser_set_do_overwrite_confirmation(IntPtr chooser, bool doOverwriteConfirmation);
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern void gtk_file_chooser_add_filter(IntPtr chooser, IntPtr filter);
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr gtk_file_chooser_dialog_new(IntPtr title, IntPtr parent, int action, IntPtr nil);
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr gtk_file_filter_new();
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern void gtk_file_filter_add_pattern(IntPtr filter,IntPtr pattern);
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern void gtk_file_filter_set_name(IntPtr filter,IntPtr name);
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern void gtk_file_chooser_set_select_multiple(IntPtr chooser,bool select_multiple);
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr gtk_file_chooser_get_filenames(IntPtr chooser);
        [DllImport(GtkApi.GtkLibraries, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr gtk_file_chooser_get_current_folder(IntPtr chooser);
        // 文件选择对话框的操作类型
        private enum FileChooserAction
        {
            Open,
            Save,
            SelectFolder,
            CreateFolder
        }

        // 响应类型
        private enum ResponseType
        {
            None = -1,
            Accept = -3,
            Cancel = -6
        }
        
        /// <summary>
        /// 返回选中文件
        /// </summary>
        /// <param name="initialDir"></param>
        /// <param name="filters"></param>
        /// <returns>fileName：文件名，fileFullName：包含路径的文件名</returns>
        public (bool Selected, string fileName, string fileFullName) OpenFile(string initialDir = null, Dictionary<string, string> filters = null) 
        {
            IntPtr fileChooser = gtk_file_chooser_dialog_new("选择文件", IntPtr.Zero, FileChooserAction.Open, 
                "打开", ResponseType.Accept, 
                "取消", ResponseType.Cancel, 
                null, ResponseType.None, IntPtr.Zero);
       
            gtk_file_chooser_set_current_folder(fileChooser, initialDir);
            if (filters == null)
            {
                filters = new Dictionary<string, string>() { { "所有文件（*.*)", "*.*" } };
            }
        foreach (var filter in filters)
        {
            var item = gtk_file_filter_new();
            gtk_file_filter_add_pattern(item, Marshal.StringToHGlobalAnsi(filter.Value));
            gtk_file_filter_set_name(item, Marshal.StringToHGlobalAnsi(filter.Key));
            gtk_file_chooser_add_filter(fileChooser, item);
        }
        if (GtkApi.gtk_dialog_run(fileChooser) == (int)ResponseType.Accept)
        {
            var selectedFilePath = Marshal.PtrToStringAuto(gtk_file_chooser_get_filename(fileChooser));
            GtkApi.gtk_widget_destroy(fileChooser);
            return (true, Path.GetFileName(selectedFilePath), selectedFilePath);
        }
        GtkApi.gtk_widget_destroy(fileChooser);return (false, "", "");
        }
        /// <summary>
        /// 返回选中文件列表
        /// </summary>
        /// <param name="initialDir"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public (bool Selected, Dictionary<string, string> Files) OpenFiles(string initialDir = null, Dictionary<string, string> filters = null)
        {

            IntPtr fileChooser = gtk_file_chooser_dialog_new("选择文件", IntPtr.Zero, FileChooserAction.Open, 
                "打开", ResponseType.Accept, 
                "取消", ResponseType.Cancel, 
                null, ResponseType.None, IntPtr.Zero);
            gtk_file_chooser_set_select_multiple(fileChooser, true);
            gtk_file_chooser_set_current_folder(fileChooser, initialDir);
            gtk_file_chooser_set_do_overwrite_confirmation(fileChooser, true);
            if (filters == null)
            {
                filters = new Dictionary<string, string>() { { "所有文件（*.*)", "*.*" } };
            }
            foreach (var filter in filters)
            {
                var item = gtk_file_filter_new();
                gtk_file_filter_add_pattern(item, Marshal.StringToHGlobalAnsi(filter.Value));
                gtk_file_filter_set_name(item, Marshal.StringToHGlobalAnsi(filter.Key));
                gtk_file_chooser_add_filter(fileChooser, item);
            }
            if (GtkApi.gtk_dialog_run(fileChooser) == (int)ResponseType.Accept)
            {
                var selectedFilePaths = gtk_file_chooser_get_filenames(fileChooser).PtrToStringList();
                GtkApi.gtk_widget_destroy(fileChooser);
                return (true, selectedFilePaths.ToDictionary(s => Path.GetFileName(s), s => s));
            }
            GtkApi.gtk_widget_destroy(fileChooser);
            return (false, null);
        }

        public (bool Selected, string fileName, string fileFullName) SaveFile(string fileName, string initialDir = null, Dictionary<string, string> filters = null)
        {
            IntPtr fileChooser = gtk_file_chooser_dialog_new("保存文件", IntPtr.Zero, FileChooserAction.Open, 
                "选择", ResponseType.Accept, 
                "取消", ResponseType.Cancel, 
                null, ResponseType.None, IntPtr.Zero);
       
            gtk_file_chooser_set_current_folder(fileChooser, initialDir);
            gtk_file_chooser_set_do_overwrite_confirmation(fileChooser, true);
            if (filters == null)
            {
                filters = new Dictionary<string, string>() { { "所有文件（*.*)", "*.*" } };
            }
            foreach (var filter in filters)
            {
                var item = gtk_file_filter_new();
                gtk_file_filter_add_pattern(item, Marshal.StringToHGlobalAnsi(filter.Value));
                gtk_file_filter_set_name(item, Marshal.StringToHGlobalAnsi(filter.Key));
                gtk_file_chooser_add_filter(fileChooser, item);
            }
            if (GtkApi.gtk_dialog_run(fileChooser) == (int)ResponseType.Accept)
            {
                var selectedFilePath = Marshal.PtrToStringAuto(gtk_file_chooser_get_filename(fileChooser));
                GtkApi.gtk_widget_destroy(fileChooser);
                return (true, Path.GetFileName(selectedFilePath), selectedFilePath);
            }
            GtkApi.gtk_widget_destroy(fileChooser);return (false, "", "");
        }

        public (bool Selected, string path) OpenDirectory(string initialDir = null)
        {
                IntPtr fileChooser = gtk_file_chooser_dialog_new("选择目录", IntPtr.Zero, FileChooserAction.SelectFolder, 
                "选择", ResponseType.Accept, 
                "取消", ResponseType.Cancel, 
                null, ResponseType.None, IntPtr.Zero);
       
            gtk_file_chooser_set_current_folder(fileChooser, initialDir);
            gtk_file_chooser_set_do_overwrite_confirmation(fileChooser, true);
            
            if (GtkApi.gtk_dialog_run(fileChooser) == (int)ResponseType.Accept)
            {
                var selectedPath = Marshal.PtrToStringAuto(gtk_file_chooser_get_current_folder(fileChooser));
                GtkApi.gtk_widget_destroy(fileChooser);
                return (true, selectedPath);
            }
            GtkApi.gtk_widget_destroy(fileChooser);
            return (false, "");

            return (false, "");
        }
    }
}
