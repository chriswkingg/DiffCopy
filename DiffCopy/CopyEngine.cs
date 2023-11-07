namespace DiffCopy;

public class CopyEngine
{
    public FileList FileList { get; init; }
    public string? RootSrc { get; init; }
    public string? RootDest { get; init; }
    public bool Running { get; private set; } = false;

    public void Start()
    {
        Running = true;
        ThreadPool.QueueUserWorkItem(Run);
    }

    public void Stop()
    {
        Running = false;
    }

    private void Run(object? obj)
    {
        while (Running)
        {
            // ThreadPool.QueueUserWorkItem(CopyFile());
            // FileList.FilePaths.RemoveAt(0);
        }
        
    }
    public static void CopyFile(string src, string dest)
    {
        var destParent = Directory.GetParent(dest);
        Directory.CreateDirectory(destParent?.ToString() ?? throw new InvalidOperationException());
        File.Copy(src, dest, true);
    }

    // *somewhat* dangerous
    public static string RemoveRoot(string path, string root) => path[(root.Length + 1)..];
}