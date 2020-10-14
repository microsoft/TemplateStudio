using System;
using System.IO;
using Param_RootNamespace.Core.Services;
using Newtonsoft.Json;
using Xunit;

namespace Param_RootNamespace.Core.Tests.xUnit
{
    public class FileServiceTests : IDisposable
    {
        private readonly string _folderPath;
        private readonly string _fileName;
        private readonly string _fileData;
        private readonly string _filePath;

        public FileServiceTests()
        {
            _folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UnitTests");
            _fileName = "Tests.json";
            _fileData = "Lorem ipsum dolor sit amet";
            _filePath = Path.Combine(_folderPath, _fileName);
        }

        [Fact]
        public void TestSaveFile()
        {
            var fileService = new FileService();

            fileService.Save(_folderPath, _fileName, _fileData);

            if (File.Exists(_filePath))
            {
                var jsonContentFile = File.ReadAllText(_filePath);
                var contentFile = JsonConvert.DeserializeObject<string>(jsonContentFile);
                Assert.Equal(_fileData, contentFile);
            }
            else
            {
                Assert.True(false, $"File not exist: {_filePath}");
            }
        }

        [Fact]
        public void TestReadFile()
        {
            var fileService = new FileService();
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(_fileData));

            var cacheData = fileService.Read<string>(_folderPath, _fileName);

            Assert.Equal(_fileData, cacheData);
        }

        [Fact]
        public void TestDeleteFile()
        {
            var fileService = new FileService();
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            File.WriteAllText(_filePath, _fileData);

            fileService.Delete(_folderPath, _fileName);

            Assert.False(File.Exists(_filePath));
        }

        public void Dispose()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
    }
}
