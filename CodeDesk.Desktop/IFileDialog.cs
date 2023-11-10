namespace CodeDesk.Desktop
{
    public interface IFileDialog
    {
        (bool Selected, string path) OpenDirectory(string initialDir = null);
        (bool Selected, string fileName, string fileFullName) OpenFile(string initialDir = null, Dictionary<string, string> filters = null);
        (bool Selected, Dictionary<string, string> Files) OpenFiles(string initialDir = null, Dictionary<string, string> filters = null);
        (bool Selected, string fileName, string fileFullName) SaveFile(string fileName, string initialDir = null, Dictionary<string, string> filters = null);
    }
}