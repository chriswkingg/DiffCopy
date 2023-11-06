namespace DiffCopy;

public class CopyEngine
{
    
    public static void CopyFile(string src, string dest)
    {
        DirectoryInfo destParent = Directory.GetParent(dest);
        Directory.CreateDirectory(destParent?.ToString());
        StreamWriter sw = new StreamWriter(src);
        File.Copy(src, dest, true);
    }
}