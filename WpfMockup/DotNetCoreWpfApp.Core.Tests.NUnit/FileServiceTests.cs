using System;
using System.IO;
using DotNetCoreWpfApp.Core.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DotNetCoreWpfApp.Core.Tests.NUnit
{
    public class FileServiceTests
    {
        private string _folderPath;
        private string _fileName;
        private string _fileData;
        private string _filePath;

        public FileServiceTests()
        {

        }

        [SetUp]
        public void Setup()
        {
            _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UnitTests");
            _fileName = "Tests.json";
            _fileData = "Lorem ipsum dolor sit amet";
            _filePath = Path.Combine(_folderPath, _fileName);
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
    }
}
