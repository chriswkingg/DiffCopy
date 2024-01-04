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
        GeneratedSourceList = new FileList();
        GeneratedSourceList.GenerateFileList(source);
        if (File.Exists(Path.Combine(source, "filelist.txt")))
        {
            ExistingSourceList = new FileList();
            ExistingSourceList.Read(Path.Combine(source, "filelist.txt"));
            if (ExistingSourceList.GenerateDiff(GeneratedSourceList).FilePaths.Count == 0)
            {
                Console.WriteLine("Currently not supporting changing source files");
                return;
            }
        }

        // Compare current file structure state to last recorded state of dest
        // If different, exit
        GeneratedDestList = new FileList();
        GeneratedDestList.GenerateFileList(dest);
        if (File.Exists(Path.Combine(dest, "filelist.txt")))
        {
            ExistingDestList = new FileList();
            ExistingDestList.Read(Path.Combine(source, "filelist.txt"));
            if (ExistingDestList.GenerateDiff(GeneratedDestList).FilePaths.Count == 0)
            {
                Console.WriteLine("Currently not supporting changing dest files");
                return;
            }
        }
        
        ce = new CopyEngine
        {
            FileList = ExistingSourceList.GenerateDiff(ExistingDestList),
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
                    break;
                case "stop":
                    // Save copy state
                    break;
            }
            
        }
    }
}