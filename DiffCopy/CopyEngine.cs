namespace DiffCopy;

public class CopyEngine
{
    public required FileList? FileList { get; init; }
    private readonly object _fileListLock = new ();
    public required string RootSrc { get; init; }
    public required string RootDest { get; init; }
    public bool Running { get; private set; }

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
        for (var i = 0; i < ThreadCount; i++)
        {
            WorkerThreads[i] = new Thread(CopyWorker);
            Console.WriteLine($"Created thread {WorkerThreads[i]?.ManagedThreadId}");
            WorkerThreads[i]?.Start();
        }
        
    }

    private void StopThreads()
    {
        for (var i = 0; i < ThreadCount; i++)
        {
            Console.WriteLine($"Waiting for thread {WorkerThreads[i]?.ManagedThreadId}");
            WorkerThreads[i]?.Join();
        }
    }

    private void CopyWorker()
    {
        while (Running)
        {
            string rootlessPath;
            lock (_fileListLock)
            {
                if (FileList.FilePaths.Count == 0)
                {
                    Running = false;
                    Console.WriteLine($"No work remaining thread: {Environment.CurrentManagedThreadId}, exiting");
                    return;
                }

                rootlessPath = FileList.NextPath();
            }

            var srcPath = Path.Combine(RootSrc, rootlessPath);
            var destPath = Path.Combine(RootDest, rootlessPath);
            Console.WriteLine($"Thread: {Environment.CurrentManagedThreadId} Copying File: {srcPath} to {destPath}");
            CopyFile(srcPath, destPath);
        }
    }
    public static void CopyFile(string src, string dest)
    {
        var destParent = Directory.GetParent(dest);
        Directory.CreateDirectory(destParent?.ToString() ?? throw new InvalidOperationException());
        File.Copy(src, dest, true);
    }
}