
namespace DiffCopy;

public class FileList
{
    // TODO: This should be private, make helper methods for needed functions such as isEmpty()
    public readonly List<string> FilePaths = new List<string>();
    public string Root { get; private set; }

    private FileList(List<string> filesPaths)
    {
        FilePaths = filesPaths;
    }

    public FileList()
    {
        
    }
    
    public void GenerateFileList(string root)
    {
        // Temporary until file list refactoring
        Root = root;
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
    public FileList GenerateDiff(FileList other) => new FileList(FilePaths.Except(other.FilePaths).ToList());

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

    public void RemoveRoot()
    {
        for(var i = 0; i < FilePaths.Count; i++)
        {
            // Is this a bad idea?? maybe!
            FilePaths[i] = FilePaths[i].Substring(Root.Length + 1, FilePaths[i].Length - Root.Length - 1);
        }
    }
}