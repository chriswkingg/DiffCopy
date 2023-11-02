
namespace DiffCopy;

public static class FileUtils
{
    public static List<string> GenerateFileList(string root)
    {
        var fileList = new List<string>();
        GenerateFileList(root, fileList);
        return fileList;
    }

    private static void GenerateFileList(string root, List<string> fileList)
    {
        foreach (var i in Directory.GetDirectories(root))
        {
            GenerateFileList(i, fileList);
        }

        foreach (var i in Directory.GetFiles(root))
        {
            fileList.Add(i);
        }
    }
    
}