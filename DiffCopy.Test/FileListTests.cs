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
    }

    [Test]
    public void TestGenerateFileList()
    {
        var testDir = Path.Combine(Directory.GetCurrentDirectory(), "test");
        FileList fileList = new FileList();
        fileList.GenerateFileList(testDir);
        Assert.AreEqual(8, fileList.FilePaths.Count);
        Assert.Contains(Path.Combine(testDir, "3/file7.txt"), fileList.FilePaths);
    }

    [Test]
    public void TestGenerateDiff()
    {
        var fileList1 = new FileList();
        fileList1.FilePaths.Add("f1");
        fileList1.FilePaths.Add("f2");
        fileList1.FilePaths.Add("f3");
        var fileList2 = new FileList();
        fileList2.FilePaths.Add("f1");
        fileList2.FilePaths.Add("f2");
        fileList2.FilePaths.Add("f3");
        fileList2.FilePaths.Add("f4");
        fileList2.FilePaths.Add("f5");
        
        var diff = fileList2.GenerateDiff(fileList1);
        Assert.AreEqual(2, diff.Count);
        Assert.AreEqual("f4", diff[0]);
        Assert.AreEqual("f5", diff[1]);
    }
}