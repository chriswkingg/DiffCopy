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
        fs = File.Create(Path.Combine(testDir, "1", "file2.txt"));
        fs.Close();
        fs = File.Create(Path.Combine(testDir, "1", "file3.txt"));
        fs.Close();
        fs = File.Create(Path.Combine(testDir, "2", "file4.txt"));
        fs.Close();
        fs = File.Create(Path.Combine(testDir, "2", "file5.txt"));
        fs.Close();
        fs = File.Create(Path.Combine(testDir, "2", "file6.txt"));
        fs.Close();
        fs = File.Create(Path.Combine(testDir, "3", "file1.txt"));
        fs.Close();
        fs = File.Create(Path.Combine(testDir, "3", "file8.txt"));
        fs.Close();
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
        File.Delete(Path.Combine(testDir, "3", "file1.txt"));
        File.Delete(Path.Combine(testDir, "3", "file8.txt"));
        Directory.Delete(Path.Combine(testDir, "1"));
        Directory.Delete(Path.Combine(testDir, "2"));
        Directory.Delete(Path.Combine(testDir, "3"));
        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "filelist.txt"));
        Directory.Delete(testDir);
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

    // Doesnt belong here
    // [Test]
    // public void TestRemoveRoot()
    // {
    //     var srcRoot = Path.Combine(Directory.GetCurrentDirectory(), "test");
    //     var rootless = CopyEngine.RemoveRoot(Path.Combine(Directory.GetCurrentDirectory(), "test", "1", "file1.txt"), srcRoot);
    //     Assert.AreEqual("/1/file1.txt", rootless);
    // }

    [Test]
    public void TestCopyEngine()
    {
        var srcPath = Path.Combine(Directory.GetCurrentDirectory(), "test");
        var destPath = Path.Combine(Directory.GetCurrentDirectory(), "test2");

        var fe = new FileList(srcPath);
        fe.GenerateFileList();
        
        CopyEngine ce = new CopyEngine
        {
            FileList = fe,
            RootSrc = srcPath,
            RootDest = destPath
        };

        ce.Start();
        
        Thread.Sleep(1000);
        
        // Did the files copy?
        Assert.True(File.Exists(Path.Combine(destPath, "1", "file1.txt")));
        Assert.True(File.Exists(Path.Combine(destPath, "1", "file2.txt")));
        Assert.True(File.Exists(Path.Combine(destPath, "1", "file3.txt")));
        Assert.True(File.Exists(Path.Combine(destPath, "2", "file4.txt")));
        Assert.True(File.Exists(Path.Combine(destPath, "2", "file5.txt")));
        Assert.True(File.Exists(Path.Combine(destPath, "2", "file6.txt")));
        Assert.True(File.Exists(Path.Combine(destPath, "3", "file1.txt")));
        Assert.True(File.Exists(Path.Combine(destPath, "3", "file8.txt")));
        
        
        // Cleanup for this specific test
        File.Delete(Path.Combine(destPath, "1", "file1.txt"));
        File.Delete(Path.Combine(destPath, "1", "file2.txt"));
        File.Delete(Path.Combine(destPath, "1", "file3.txt"));
        File.Delete(Path.Combine(destPath, "2", "file4.txt"));
        File.Delete(Path.Combine(destPath, "2", "file5.txt"));
        File.Delete(Path.Combine(destPath, "2", "file6.txt"));
        File.Delete(Path.Combine(destPath, "3", "file1.txt"));
        File.Delete(Path.Combine(destPath, "3", "file8.txt"));
        Directory.Delete(Path.Combine(destPath, "1"));
        Directory.Delete(Path.Combine(destPath, "2"));
        Directory.Delete(Path.Combine(destPath, "3"));
        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "filelist.txt"));
        Directory.Delete(destPath);
    }
}