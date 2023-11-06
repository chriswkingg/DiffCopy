
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

    public void Write(string path)
    {
        using StreamWriter file = (File.Exists(path)) ? File.AppendText(path) : File.CreateText(path);
        foreach (var i in FilePaths)
        {
            file.WriteLine(i);
        }
    }

    public bool Read(string path)
    {
        if (!File.Exists(path)) return false;
        using var file = new StreamReader(path, true);
        while (file.ReadLine() is { } line)
        {
            FilePaths.Add(line);
        }

        return true;
    }
}