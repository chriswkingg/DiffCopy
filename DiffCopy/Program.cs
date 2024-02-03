using Console = System.Console;

namespace DiffCopy;

public static class Program
{
    public static CopyEngine ce;
    public static FileList? ExistingSourceList, GeneratedSourceList, GeneratedDestList, ExistingDestList;
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter source folder");
        var source = Console.ReadLine();
        Console.WriteLine("Enter dest folder");
        var dest = Console.ReadLine();
        
        // Compare current file structure state to last recorded state of source
        // If different, exit
        GeneratedSourceList = new FileList(source);
        GeneratedSourceList.GenerateFileList();
        if (File.Exists(Path.Combine(source, "filelist.txt")))
        {
            ExistingSourceList = new FileList(source);
            ExistingSourceList.Read(Path.Combine(source, "filelist.txt"));
            if (ExistingSourceList.GenerateDiff(GeneratedSourceList).FilePaths.Count == 0)
            {
                Console.WriteLine("Currently not supporting changing source files");
                return;
            }
        }

        // Compare current file structure state to last recorded state of dest
        // If different, exit
        GeneratedDestList = new FileList(dest);
        GeneratedDestList.GenerateFileList();
        if (File.Exists(Path.Combine(dest, "filelist.txt")))
        {
            ExistingDestList = new FileList(dest);
            ExistingDestList.Read(Path.Combine(source, "filelist.txt"));
            if (ExistingDestList.GenerateDiff(GeneratedDestList).FilePaths.Count == 0)
            {
                Console.WriteLine("Currently not supporting changing dest files");
                return;
            }
        }
        
        ce = new CopyEngine
        {
            FileList = GeneratedSourceList.GenerateDiff(GeneratedDestList),
            RootSrc = source,
            RootDest = dest
        };
        StartConsole();
    }

    public static  void StartConsole()
    {
        while (true)
        {
            var line = Console.ReadLine();
            if (line == null) return;
            var parameters = line.Split();
            switch (parameters[0])
            {
                case "start":
                    ce.Start();
                    break;
                case "stop":
                    ce.Stop();
                    break;
                case "exit":
                    return;
            }
            
        }
    }
}