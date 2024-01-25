using NUnit.Framework;

namespace DiffCopy.Test;

[TestFixture]
public class FileListTests
{
    [OneTimeSetUp]
    public void GenerateTestFileStructure()
    {
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
        Directory.CreateDirectory(Path.Combine(testDir, "1"));
        Directory.CreateDirectory(Path.Combine(testDir, "2"));
        Directory.CreateDirectory(Path.Combine(testDir, "3"));
        File.Create(Path.Combine(testDir, "1", "file1.txt"));
        File.Create(Path.Combine(testDir, "1", "file2.txt"));
        File.Create(Path.Combine(testDir, "1", "file3.txt"));
        File.Create(Path.Combine(testDir, "2", "file4.txt"));
        File.Create(Path.Combine(testDir, "2", "file5.txt"));
        File.Create(Path.Combine(testDir, "2", "file6.txt"));
        File.Create(Path.Combine(testDir, "3", "file1.txt"));
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
        File.Delete(Path.Combine(testDir, "3", "file1.txt"));
        File.Delete(Path.Combine(testDir, "3", "file8.txt"));
        Directory.Delete(Path.Combine(testDir, "1"));
        Directory.Delete(Path.Combine(testDir, "2"));
        Directory.Delete(Path.Combine(testDir, "3"));
        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "filelist.txt"));
    }

    [Test]
    public void TestGenerateFileList()
    {
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
        FileList fileList = new FileList(testDir);
        fileList.GenerateFileList();
        Assert.AreEqual(8, fileList.FilePaths.Count);
        // Contains only works for ICollection and a readonly list or readonly collection isnt an ICollection?
        Assert.Contains(Path.Combine(testDir, "3/file1.txt"), fileList.FilePaths.ToList());
    }

    [Test]
    public void TestGenerateDiff()
    {
        var fileList1 = new FileList(Path.Combine(Directory.GetCurrentDirectory(), "test", "1"));
        fileList1.GenerateFileList();
        fileList1.RemoveRoots();
        var fileList2 = new FileList(Path.Combine(Directory.GetCurrentDirectory(), "test", "3"));
        fileList2.GenerateFileList();
        fileList2.RemoveRoots();
        
        var diff = fileList2.GenerateDiff(fileList1);
        Assert.AreEqual(1, diff.FilePaths.Count);
        Assert.AreEqual("file8.txt", diff.FilePaths[0]);
    }

    [Test]
    public void TestWrite()
    {
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
        FileList fileList = new (Path.Combine(Directory.GetCurrentDirectory(), "test"));
        fileList.GenerateFileList();
        var fileListTxt = Path.Combine(Directory.GetCurrentDirectory(), "filelist.txt");
        fileList.Write(fileListTxt);
        Assert.IsTrue(File.Exists(fileListTxt));
    }

    [Test]
    public void TestRead()
    {
        TestWrite();
        var fileListTxt = Path.Combine(Directory.GetCurrentDirectory(), "filelist.txt");
        FileList fileList = new (Path.Combine(Directory.GetCurrentDirectory()));
        var successful = fileList.Read(fileListTxt);
        Assert.True(successful);
        Assert.True(fileList.FilePaths.Contains(Path.Combine(Directory.GetCurrentDirectory(), "test", "1", "file2.txt")));
        Assert.True(fileList.FilePaths.Contains(Path.Combine(Directory.GetCurrentDirectory(), "test", "1", "file3.txt")));
        Assert.True(fileList.FilePaths.Contains(Path.Combine(Directory.GetCurrentDirectory(), "test", "2", "file6.txt")));
    }

    [Test]
    public void TestRemoveRoot()
    {
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
        FileList fileList = new FileList(testDir);
        fileList.GenerateFileList();
        fileList.RemoveRoots();
        Assert.Contains("3/file1.txt", fileList.FilePaths.ToList());
        Assert.Contains("1/file1.txt", fileList.FilePaths.ToList());
    }
}