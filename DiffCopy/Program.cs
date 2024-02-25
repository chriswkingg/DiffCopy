
namespace DiffCopy;

public static class Program
{
    private static CopyEngine ce;
    public static FileList? ExistingSourceList, GeneratedSourceList, GeneratedDestList, ExistingDestList;
    public static string SourcePath, DestPath;
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter source folder");
        SourcePath = Console.ReadLine();
        Console.WriteLine("Enter dest folder");
        DestPath = Console.ReadLine();

        // TODO: More to come for both comp operations, warn the user, ask what they want to do
        CompareSourceLists();
        CompareDestLists();
        ce = new CopyEngine
        {
            FileList = GeneratedSourceList.GenerateDiff(GeneratedDestList),
            RootSrc = SourcePath,
            RootDest = DestPath
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
                    GeneratedSourceList = new FileList(SourcePath);
                    GeneratedSourceList.GenerateFileList();
                    GeneratedSourceList.Write(Path.Combine(SourcePath, "filelist.txt"));
                    GeneratedDestList = new FileList(DestPath);
                    GeneratedDestList.GenerateFileList();
                    GeneratedDestList.Write(Path.Combine(DestPath, "filelist.txt"));
                    break;
                case "exit":
                    return;
            }
            
        }
    }

    private static void CompareSourceLists()
    {
        // Compare current file structure state to last recorded state of source
        GeneratedSourceList = new FileList(SourcePath);
        GeneratedSourceList.GenerateFileList();
        if (File.Exists(Path.Combine(SourcePath, "filelist.txt")))
        {
            ExistingSourceList = new FileList(SourcePath);
            ExistingSourceList.Read(Path.Combine(SourcePath, "filelist.txt"));
            FileList difference = GeneratedSourceList.GenerateDiff(ExistingSourceList);
            if (difference.FilePaths.Count != 0)
            {
                Console.WriteLine("The following files have been added since the last file list generation");
                foreach (var i in difference.FilePaths)
                {
                    Console.WriteLine(i);
                }
            }
            difference = ExistingSourceList.GenerateDiff(GeneratedSourceList);
            if (difference.FilePaths.Count != 0)
            {
                Console.WriteLine("The following files have been deleted since the last file list generation");
                foreach (var i in difference.FilePaths)
                {
                    Console.WriteLine(i);
                }
            }
        }
        // Save the generated file list
        GeneratedSourceList.Write(Path.Combine(SourcePath, "filelist.txt"));
    }

    private static void CompareDestLists()
    {
        // Compare current file structure state to last recorded state of dest
        // If different, exit
        GeneratedDestList = new FileList(DestPath);
        GeneratedDestList.GenerateFileList();
        if (File.Exists(Path.Combine(DestPath, "filelist.txt")))
        {
            ExistingDestList = new FileList(DestPath);
            ExistingDestList.Read(Path.Combine(DestPath, "filelist.txt"));
            if (ExistingDestList.GenerateDiff(GeneratedDestList).FilePaths.Count != 0)
            {
                Console.WriteLine("Currently not supporting changing dest files");
                return;
            }
        }
        
        // Save the generated file list
        GeneratedDestList.Write(Path.Combine(DestPath, "filelist.txt"));

    }
}