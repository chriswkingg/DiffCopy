namespace DiffCopy;

public class CopyEngine
{
    public FileList FileList { get; init; }
    private object FileListLock = new ();
    public string? RootSrc { get; init; }
    public string? RootDest { get; init; }
    public bool Running { get; private set; } = false;

    public const int ThreadCount = 1;
    private Thread?[] WorkerThreads { get; set; } = new Thread?[ThreadCount];

    public void Start()
    {
        Running = true;
        StartThreads();
    }

    public void Stop()
    {
        Running = false;
        StopThreads();  
    }

    private void StartThreads()
    {
        while (Running)
        {
            for(var i = 0; i < ThreadCount; i++)
            {
                WorkerThreads[i] = new Thread(CopyWorker);
                WorkerThreads[i]?.Start();
            }
        }
    }

    private void StopThreads()
    {
        for (var i = 0; i < ThreadCount; i++)
        {
            WorkerThreads[i]?.Join();
        }
    }

    private void CopyWorker()
    {
        while (Running)
        {
            var srcPath = "";
            lock (FileListLock)
            {
                if (FileList.FilePaths.Count == 0)
                {
                    Running = false;
                    return;
                }
                srcPath = FileList.FilePaths[0];
                FileList.FilePaths.RemoveAt(0);
            }

            var destPath = RootDest + RemoveRoot(srcPath, RootSrc);
            CopyFile(srcPath, destPath);
        }
    }
    public static void CopyFile(string src, string dest)
    {
        var destParent = Directory.GetParent(dest);
        Directory.CreateDirectory(destParent?.ToString() ?? throw new InvalidOperationException());
        File.Copy(src, dest, true);
    }

    // *somewhat* dangerous
    public static string RemoveRoot(string path, string root) => path[(root.Length)..];
}