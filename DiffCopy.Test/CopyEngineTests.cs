using NUnit.Framework;

namespace DiffCopy.Test;

[TestFixture]
public class CopyEngineTests
{
    [OneTimeSetUp]
    public void GenerateTestFileStructure()
    {
        FileStream fs;
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
        Directory.CreateDirectory(Path.Combine(testDir, "1"));
        Directory.CreateDirectory(Path.Combine(testDir, "2"));
        Directory.CreateDirectory(Path.Combine(testDir, "3"));
        // Apparently file.create keeps files open, this causes problems for TestCopyFile()
        fs = File.Create(Path.Combine(testDir, "1", "file1.txt"));
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
        fs = File.Create(Path.Combine(testDir, "3", "file7.txt"));
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

    [Test]
    public void TestRemoveRoot()
    {
        var srcRoot = Path.Combine(Directory.GetCurrentDirectory(), "test");
        var rootless = CopyEngine.RemoveRoot(Path.Combine(Directory.GetCurrentDirectory(), "test", "1", "file1.txt"), srcRoot);
        Assert.AreEqual("/1/file1.txt", rootless);
    }

    [Test]
    public void TestCopyEngine()
    {
        var srcpath = Path.Combine(Directory.GetCurrentDirectory(), "test");
        var destpath = Path.Combine(Directory.GetCurrentDirectory(), "test2");

        var fe = new FileList(srcpath);
        fe.GenerateFileList();
        
        CopyEngine ce = new CopyEngine
        {
            FileList = fe,
            RootSrc = srcpath,
            RootDest = destpath
        };

        ce.Start();
        
        Thread.Sleep(1000);
        
        // Did the files copy?
        Assert.True(File.Exists(Path.Combine(destpath, "1", "file1.txt")));
        Assert.True(File.Exists(Path.Combine(destpath, "1", "file2.txt")));
        Assert.True(File.Exists(Path.Combine(destpath, "1", "file3.txt")));
        Assert.True(File.Exists(Path.Combine(destpath, "2", "file4.txt")));
        Assert.True(File.Exists(Path.Combine(destpath, "2", "file5.txt")));
        Assert.True(File.Exists(Path.Combine(destpath, "2", "file6.txt")));
        Assert.True(File.Exists(Path.Combine(destpath, "3", "file7.txt")));
        Assert.True(File.Exists(Path.Combine(destpath, "3", "file8.txt")));
        
        
        // Cleanup
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test2");
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
}