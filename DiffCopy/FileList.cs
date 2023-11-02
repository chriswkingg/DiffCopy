
namespace DiffCopy;

public class FileList
{
    public readonly List<string> FilePaths = new List<string>();
    public void GenerateFileList(string root)
    {
        GenerateFileList(root, FilePaths);
    }

    private static void GenerateFileList(string root, List<string> fileList)
    {
        foreach (var i in Directory.GetDirectories(root))
        {
            GenerateFileList(i, fileList);
        }

        fileList.AddRange(Directory.GetFiles(root));
    }

    public List<string> GenerateDiff(FileList other) => FilePaths.Except(other.FilePaths).ToList();
    
    
}