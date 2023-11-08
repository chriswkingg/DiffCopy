using Console = System.Console;

namespace DiffCopy;

public static class Program
{
    public static CopyEngine ce;
    public static FileList fl;
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter source folder");
        var source = Console.ReadLine();
        Console.WriteLine("Enter dest folder");
        var dest = Console.ReadLine();
        fl = new FileList();
        fl.GenerateFileList(source);
        ce = new()
        {
            FileList = null,
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
                    break;
            }
            
        }
    }
}