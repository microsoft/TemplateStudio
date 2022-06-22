using System.IO;
using Param_RootNamespace.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Param_RootNamespace.Core.Tests.MSTest;

[TestClass]
public class FileServiceTests
{
    private string _folderPath;
    private string _fileName;
    private string _fileData;
    private string _filePath;

    public FileServiceTests()
    {
    }

    [TestInitialize]
    public void Setup()
    {
        _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UnitTests");
        _fileName = "Tests.json";
        _fileData = "Lorem ipsum dolor sit amet";
        _filePath = Path.Combine(_folderPath, _fileName);
    }

    [TestMethod]
    public void TestSaveFile()
    {
        var fileService = new FileService();

        fileService.Save(_folderPath, _fileName, _fileData);

        if (File.Exists(_filePath))
        {
            var jsonContentFile = File.ReadAllText(_filePath);
            var contentFile = JsonConvert.DeserializeObject<string>(jsonContentFile);
            Assert.AreEqual(_fileData, contentFile);
        }
        else
        {
            Assert.Fail($"File not exist: {_filePath}");
        }
    }

    [TestMethod]
    public void TestReadFile()
    {
        var fileService = new FileService();
        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        File.WriteAllText(_filePath, JsonConvert.SerializeObject(_fileData));

        var cacheData = fileService.Read<string>(_folderPath, _fileName);

        Assert.AreEqual(_fileData, cacheData);
    }

    [TestMethod]
    public void TestDeleteFile()
    {
        var fileService = new FileService();
        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        File.WriteAllText(_filePath, _fileData);

        fileService.Delete(_folderPath, _fileName);

        Assert.IsFalse(File.Exists(_filePath));
    }

    [TestCleanup]
    public void TestCleanup()
    {
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
}
