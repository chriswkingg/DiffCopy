using Console = System.Console;

namespace DiffCopy;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter source folder");
        var source = Console.ReadLine();
        Console.WriteLine("Enter dest folder");
        var dest = Console.ReadLine();
    }
}