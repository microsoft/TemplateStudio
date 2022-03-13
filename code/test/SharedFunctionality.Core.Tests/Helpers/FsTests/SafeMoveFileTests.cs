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
    public class SafeMoveFileTests : IDisposable
    {
        private readonly FSTestsFixture _fixture;
        private readonly string _testFolder;

        private DateTime _logDate;

        private const string ErrorMessage = "can't be moved to";
        private const string ErrorLevel = "Warning";

        public SafeMoveFileTests(FSTestsFixture fixture)
        {
            _fixture = fixture;
            Thread.CurrentThread.CurrentUICulture = fixture.CultureInfo;
            _testFolder = _fixture.CreateTempFolderForTest("SafeMoveFile");
        }

        [Fact]
        public void SafeMoveFile_ValidData_ShouldMove()
        {
            var testScenarioName = "ValidData";
            var originalFile = $"{_testFolder}\\{testScenarioName}_Original.cs";
            var movedFile = $"{_testFolder}\\{testScenarioName}_Moved.cs";

            FSTestsFixture.CreateFiles(_testFolder, new List<string> { Path.GetFileName(originalFile) });

            var originFolderTotalFiles = Directory.GetFiles(_testFolder).Length;

            Fs.SafeMoveFile(originalFile, movedFile);

            var destinationFolderTotalFiles = Directory.GetFiles(_testFolder).Length;

            Assert.True(originFolderTotalFiles == destinationFolderTotalFiles);
            Assert.False(File.Exists(originalFile));
            Assert.True(File.Exists(movedFile));
        }

        [Theory]
        [InlineData("", "anything")]
        [InlineData(null, "anything")]
        public void SafeMoveFile_OriginFileDoesNotExist_JustReturns(string filePath, string newfilePath)
        {
            Fs.SafeMoveFile(filePath, newfilePath);
        }

        [Fact]
        public void SafeMoveFile_DestFileExists_Overwrite_MovesFileSuccessfully()
        {
            var testScenarioName = "DestFileExists_Overwrite";
            var originalFile = $"{_testFolder}\\{testScenarioName}_Original.cs";
            var movedFile = $"{_testFolder}\\{testScenarioName}_Moved.cs";

            FSTestsFixture.CreateFiles(_testFolder, new List<string> { Path.GetFileName(originalFile), Path.GetFileName(movedFile) });

            var originFolderTotalFiles = Directory.GetFiles(_testFolder).Length;

            Fs.SafeMoveFile(originalFile, movedFile);

            var destinationFolderTotalFiles = Directory.GetFiles(_testFolder).Length;

            Assert.True(originFolderTotalFiles > destinationFolderTotalFiles);
            Assert.False(File.Exists(originalFile));
            Assert.True(File.Exists(movedFile));
        }

        [Fact]
        public void SafeMoveFile_DestFileExists_NoOverwrite_JustReturns()
        {
            var testScenarioName = "DestFileExists_NoOverwrite";
            var originalFile = $"{_testFolder}\\{testScenarioName}_Original.cs";
            var movedFile = $"{_testFolder}\\{testScenarioName}_Moved.cs";

            FSTestsFixture.CreateFiles(_testFolder, new List<string> { Path.GetFileName(originalFile), Path.GetFileName(movedFile) });

            var originFolderTotalFiles = Directory.GetFiles(_testFolder).Length;

            Fs.SafeMoveFile(originalFile, movedFile, false);

            var destinationFolderTotalFiles = Directory.GetFiles(_testFolder).Length;

            Assert.True(originFolderTotalFiles == destinationFolderTotalFiles);
            Assert.True(File.Exists(originalFile));
            Assert.True(File.Exists(movedFile));
        }

        [Fact]
        public void SafeMoveFile_DestFileExists_Overwrite_NoPermissions_ShouldLogException()
        {
            var testScenarioName = "DestFileExists_Overwrite_NoPermissions";
            var originalFile = $"{_testFolder}\\{testScenarioName}_Original.cs";
            var movedFile = $"{_testFolder}\\{testScenarioName}_Moved.cs";

            FSTestsFixture.CreateFiles(_testFolder, new List<string> { Path.GetFileName(originalFile), Path.GetFileName(movedFile) }, true);

            try
            {
                _logDate = DateTime.Now;
                Fs.SafeMoveFile(originalFile, movedFile);

                Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
            }
            finally
            {
                _ = new FileInfo(movedFile)
                {
                    IsReadOnly = false,
                };
            }
        }

        [Fact]
        public void SafeMoveFile_DestFileExists_Overwrite_NoPermissions_NoWarnOnFailure_ShouldNotLogException()
        {
            var testScenarioName = "DestFileExists_Overwrite_NoPermissions_NoWarnOnFailure";
            var originalFile = $"{_testFolder}\\{testScenarioName}_Original.cs";
            var movedFile = $"{_testFolder}\\{testScenarioName}_Moved.cs";

            FSTestsFixture.CreateFiles(_testFolder, new List<string> { Path.GetFileName(originalFile), Path.GetFileName(movedFile) }, true);

            try
            {
                _logDate = DateTime.Now;
                Fs.SafeMoveFile(originalFile, movedFile, true, false);

                Assert.False(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, $"{Path.GetFileName(movedFile)} {ErrorMessage}"));
            }
            finally
            {
                _ = new FileInfo(movedFile)
                {
                    IsReadOnly = false,
                };
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Testing purposes only")]
        public void Dispose()
        {
            FSTestsFixture.DeleteTempFolderForTest(_testFolder);
        }
    }
}
