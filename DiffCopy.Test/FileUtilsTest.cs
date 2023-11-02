using NUnit.Framework;

namespace DiffCopy.Test;

[TestFixture]
public class FileUtilsTest
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
        var fileList = FileUtils.GenerateFileList(testDir);
        Assert.AreEqual(fileList.Count, 8);
        Assert.Contains(Path.Combine(testDir, "3/file7.txt"), fileList);
    }
}