using NUnit.Framework;

namespace DiffCopy.Test;

[TestFixture]
public class CopyEngineTests
{
    [OneTimeSetUp]
    public void GenerateTestFileStructure()
    {
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
        Directory.CreateDirectory(Path.Combine(testDir, "1"));
        Directory.CreateDirectory(Path.Combine(testDir, "2"));
        Directory.CreateDirectory(Path.Combine(testDir, "3"));
        // Apparently file.create keeps files open, this causes problems for TestCopyFile()
        var fs = File.Create(Path.Combine(testDir, "1", "file1.txt"));
        fs.Close();
        File.Create(Path.Combine(testDir, "1", "file2.txt"));
        File.Create(Path.Combine(testDir, "1", "file3.txt"));
        File.Create(Path.Combine(testDir, "2", "file4.txt"));
        File.Create(Path.Combine(testDir, "2", "file5.txt"));
        File.Create(Path.Combine(testDir, "2", "file6.txt"));
        File.Create(Path.Combine(testDir, "3", "file7.txt"));
        File.Create(Path.Combine(testDir, "3", "file8.txt"));
    }
    
    [OneTimeTearDown]
    public void DeleteTestFileStructure()
    {
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
        File.Delete(Path.Combine(testDir, "1", "file1.txt"));
        File.Delete(Path.Combine(testDir, "1", "file2.txt"));
        File.Delete(Path.Combine(testDir, "1", "file3.txt"));
        File.Delete(Path.Combine(testDir, "2", "file4.txt"));
        File.Delete(Path.Combine(testDir, "2", "file5.txt"));
        File.Delete(Path.Combine(testDir, "2", "file6.txt"));
        File.Delete(Path.Combine(testDir, "3", "file7.txt"));
        File.Delete(Path.Combine(testDir, "3", "file8.txt"));
        Directory.Delete(Path.Combine(testDir, "1"));
        Directory.Delete(Path.Combine(testDir, "2"));
        Directory.Delete(Path.Combine(testDir, "3"));
        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "filelist.txt"));
    }
    
    [Test]
    public void TestCopyFile()
    {
        var src = Path.Combine(Directory.GetCurrentDirectory(), "test", "1", "file1.txt");
        var dest = Path.Combine(Directory.GetCurrentDirectory(), "test", "1", "file1COPY.txt");
        CopyEngine.CopyFile(src, dest);
        Assert.True(File.Exists(dest));
        // Clean up
        File.Delete(dest);
    }
}