
namespace DiffCopy;

public class FileList
{
    public IReadOnlyList<string> FilePaths => _filePaths;
    private readonly List<string> _filePaths = new List<string>();
    public string Root { get; private set; }

    private FileList(string root, List<string> filesPaths)
    {
        _filePaths = filesPaths;
        Root = root;
    }

    public FileList(string root)
    {
        Root = root;
    }
    
    public void GenerateFileList()
    {
        GenerateFileList(Root, _filePaths);
        RemoveRoots();
    }

    private static void GenerateFileList(string root, List<string> fileList)
    {
        foreach (var i in Directory.GetDirectories(root))
        {
            GenerateFileList(i, fileList);
        }

        fileList.AddRange(Directory.GetFiles(root));
    }
    public FileList GenerateDiff(FileList other) => new(Root, _filePaths.Except(other._filePaths).ToList());

    public void Write(string path)
    {
        // We should not append to this file, ALWAYS load first and rewrite if needed
        using StreamWriter file = File.CreateText(path);
        foreach (var i in _filePaths)
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
            _filePaths.Add(line);
        }

        return true;
    }

    private void RemoveRoots()
    {
        for(var i = 0; i < _filePaths.Count; i++)
        {
            _filePaths[i] = _filePaths[i].Substring(Root.Length + 1, _filePaths[i].Length - Root.Length - 1);
        }
    }

    public string NextPath()
    {
        var value = _filePaths[0];
        _filePaths.RemoveAt(0);
        return value;
    }
}