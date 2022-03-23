// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Test.Helpers.FsTests.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Collection("Unit Test Logs")]
    [Trait("Group", "Minimum")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Testing purposes only")]
    public class SafeCopyFileTests : IDisposable
    {
        private readonly FSTestsFixture _fixture;
        private readonly string _testFolder;
        private readonly string _sourceFile;

        private DateTime _logDate;

        private const string ErrorMessage = "can't be copied to";
        private const string ErrorLevel = "Warning";

        public SafeCopyFileTests(FSTestsFixture fixture)
        {
            _fixture = fixture;
            Thread.CurrentThread.CurrentUICulture = fixture.CultureInfo;

            _sourceFile = $"TestData\\TestProject\\Test.csproj";
            _testFolder = _fixture.CreateTempFolderForTest("SafeCopyFile");
        }

        [Fact]
        public void SafeCopyFile_DestinationDirectoryDoesNotExist_ShouldCreateDirectory()
        {
            var testScenarioName = "DestinationDirectoryDoesNotExist";
            var directoryToCreate = $"{_testFolder}\\{testScenarioName}";
            var directoryExistsAtStart = Directory.Exists(directoryToCreate);
            var totalOriginalDirectories = Directory.GetParent(directoryToCreate).GetDirectories().Length;

            Fs.SafeCopyFile(_sourceFile, directoryToCreate, true);

            var totalNewDirectories = Directory.GetParent(directoryToCreate).GetDirectories().Length;

            var directoryHasBeenCreated = totalNewDirectories > totalOriginalDirectories;

            Assert.False(directoryExistsAtStart);
            Assert.True(directoryHasBeenCreated);
            Assert.True(Directory.Exists(directoryToCreate));
        }

        [Fact]
        public void SafeCopyFile_FileDoesNotExist_ShouldCreateNewFileWhileCopying()
        {
            var testScenarioName = "FileDoesNotExist";
            var directoryToCreate = $"{_testFolder}\\{testScenarioName}";
            FSTestsFixture.CreateFolders(_testFolder, new List<string>() { testScenarioName });
            var expectedDestinationFile = Path.Combine(directoryToCreate, Path.GetFileName(_sourceFile));

            var fileExistsAtStart = File.Exists(expectedDestinationFile);
            var totalOriginalFiles = Directory.GetFiles(directoryToCreate).Length;

            Fs.SafeCopyFile(_sourceFile, directoryToCreate, true);

            var totalNewFiles = Directory.GetFiles(directoryToCreate).Length;

            var fileHasBeenCreated = totalNewFiles > totalOriginalFiles;
            Assert.False(fileExistsAtStart);
            Assert.True(fileHasBeenCreated);
            Assert.True(File.Exists(expectedDestinationFile));
        }

        [Fact]
        public void SafeCopyFile_DestinationDirectoryAlreadyExists_ShouldNotCreateDirectory()
        {
            var testScenarioName = "DestinationDirectoryAlreadyExists";
            var directoryToCopyFile = $"{_testFolder}\\{testScenarioName}";
            FSTestsFixture.CreateFolders(_testFolder, new List<string>() { testScenarioName });
            var totalOriginalDirectories = Directory.GetParent(directoryToCopyFile).GetDirectories().Length;

            Fs.SafeCopyFile(_sourceFile, directoryToCopyFile, true);

            var totalNewDirectories = Directory.GetParent(directoryToCopyFile).GetDirectories().Length;

            var noDirectoryHasBeenCreated = totalOriginalDirectories == totalNewDirectories;

            Assert.True(noDirectoryHasBeenCreated);
        }

        [Fact]
        public void SafeCopyFile_FileAlreadyExists_ShouldNotCreateNewFileWhileCopying()
        {
            var testScenarioName = "FileAlreadyExists";
            var directoryToCopyFile = $"{_testFolder}\\{testScenarioName}";
            var totalOriginalFiles = Directory.GetParent(directoryToCopyFile).GetFiles().Length;
            var expectedDestinationFile = Path.Combine(directoryToCopyFile, Path.GetFileName(_sourceFile));
            var fileInfo = new FileInfo(expectedDestinationFile);
            var originalLastModificationTime = fileInfo.LastWriteTime;

            Fs.SafeCopyFile(_sourceFile, directoryToCopyFile, true);

            var totalNewFiles = Directory.GetParent(directoryToCopyFile).GetFiles().Length;

            var noFileHasBeenCreated = totalNewFiles == totalOriginalFiles;
            fileInfo = new FileInfo(expectedDestinationFile);
            var updatedLastModificationTime = fileInfo.LastWriteTime;

            Assert.True(noFileHasBeenCreated);
            Assert.NotEqual(originalLastModificationTime, updatedLastModificationTime);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SafeCopyFile_SourceFileNullOrEmpty_ShouldLogException(string filePath)
        {
            var testScenarioName = "SourceFileNullOrEmpty";
            var directoryToCopyFile = $"{_testFolder}\\{testScenarioName}";
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(filePath, directoryToCopyFile, true);

            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void SafeCopyFile_CouldNotFindFile_ShouldLogException()
        {
            var testScenarioName = "CouldNotFindFile";
            var directoryToCopyFile = $"{_testFolder}\\{testScenarioName}";
            var sourceFileDoesNotExist = $"{_testFolder}\\FileDoNotExists.csproj";
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(sourceFileDoesNotExist, directoryToCopyFile, true);

            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void SafeCopyFile_AccessToPathDenied_ShouldLogException()
        {
            // to force an exception while trying to copy a file. File without permissions instead of valid folder
            var testScenarioName = "AccessToPathDenied";
            var directoryToCopyFile = $"{_testFolder}\\{testScenarioName}";
            var sourceFileDoesNotHavePermissions = Environment.CurrentDirectory;
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(sourceFileDoesNotHavePermissions, directoryToCopyFile, true);

            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void SafeCopyFile_AccessToPathDenied_OvewriteFalse_ShouldLogException()
        {
            var testScenarioName = "AccessToPathDenied_OvewriteFalse";
            var directoryToCopyFile = $"{_testFolder}\\{testScenarioName}";
            var sourceFileDoesNotHavePermissions = Environment.CurrentDirectory;
            _logDate = DateTime.Now;

            Fs.SafeCopyFile(sourceFileDoesNotHavePermissions, directoryToCopyFile, false);

            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Testing purposes only")]
        public void Dispose()
        {
            FSTestsFixture.DeleteTempFolderForTest(_testFolder);
        }
    }
}
