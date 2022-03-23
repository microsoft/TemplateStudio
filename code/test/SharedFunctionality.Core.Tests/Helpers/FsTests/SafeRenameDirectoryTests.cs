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
    public class SafeRenameDirectoryTests : IDisposable
    {
        private readonly FSTestsFixture _fixture;
        private readonly string _testFolder;

        private DateTime _logDate;

        private const string ErrorMessage = "can't be renamed";
        private const string ErrorLevel = "Warning";

        public SafeRenameDirectoryTests(FSTestsFixture fixture)
        {
            _fixture = fixture;
            Thread.CurrentThread.CurrentUICulture = fixture.CultureInfo;
            _testFolder = _fixture.CreateTempFolderForTest("SafeRenameDirectory");
        }

        [Fact]
        public void SafeRenameDirectory_ValidData_ShouldMoveDirectory()
        {
            var testScenarioName = "ValidData";
            var originalDirectory = $"{_testFolder}\\{testScenarioName}_Original";
            var renamedDirectory = $"{_testFolder}\\{testScenarioName}_Renamed";

            FSTestsFixture.CreateFolders(_testFolder, new List<string> { $"{testScenarioName}_Original" });

            var totalOriginalDirectories = Directory.GetParent(originalDirectory).GetDirectories().Length;

            Fs.SafeRenameDirectory(originalDirectory, renamedDirectory);

            var totalNewDirectories = Directory.GetParent(originalDirectory).GetDirectories().Length;

            var sameNumberOfDirectories = totalNewDirectories == totalOriginalDirectories;
            Assert.True(sameNumberOfDirectories);
            var oldDirectoryHasBeenMovedToNewDirectory = Directory.Exists(renamedDirectory) && !Directory.Exists(originalDirectory);
            Assert.True(oldDirectoryHasBeenMovedToNewDirectory);
        }

        [Theory]
        [InlineData("", "anything")]
        [InlineData(null, "anything")]
        public void SafeRenameDirectory_DoesNotExist_ShouldNotThrowException(string rootDir, string newRootDir)
        {
            Fs.SafeRenameDirectory(rootDir, newRootDir);
        }

        [Fact]
        public void SafeRenameDirectory_DirectoryAlreadyExists_ShouldLogException()
        {
            var testScenarioName = "ValidData";
            var originalDirectory = $"{_testFolder}\\{testScenarioName}_Original";
            var renamedDirectory = $"{_testFolder}\\{testScenarioName}_Renamed";

            FSTestsFixture.CreateFolders(_testFolder, new List<string> { $"{testScenarioName}_Original", $"{testScenarioName}_Renamed" });

            var totalOriginalDirectories = Directory.GetParent(originalDirectory).GetDirectories().Length;

            _logDate = DateTime.Now;
            Fs.SafeRenameDirectory(originalDirectory, renamedDirectory);

            var totalNewDirectories = Directory.GetParent(originalDirectory).GetDirectories().Length;

            var sameNumberOfDirectories = totalNewDirectories == totalOriginalDirectories;
            Assert.True(sameNumberOfDirectories);
            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, $"{testScenarioName}_Original {ErrorMessage}"));
        }

        [Fact]
        public void SafeRenameDirectory_WrongDestinationFolder_ShouldLogException()
        {
            // to throw the exception we create a file with the same name we try to create the new directory
            var testScenarioName = "WrongDestinationFolder";
            var originalDirectory = $"{_testFolder}\\{testScenarioName}_Original";
            var wrongDirectory = $"{_testFolder}\\{testScenarioName}_WrongFolder.cs";

            FSTestsFixture.CreateFolders(_testFolder, new List<string> { $"{testScenarioName}_Original", $"{testScenarioName}_Renamed" });
            FSTestsFixture.CreateFiles(_testFolder, new List<string> { $"{testScenarioName}_WrongFolder.cs" });

            _logDate = DateTime.Now;
            Fs.SafeRenameDirectory(originalDirectory, wrongDirectory);

            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, $"{testScenarioName}_Original {ErrorMessage}"));
        }

        [Fact]
        public void SafeRenameDirectory_WrongDestinationFolder_WarnOnFailureFalse_ShouldNotLogError()
        {
            // to throw the exception we create a file with the same name we try to create the new directory
            var testScenarioName = "WrongDestinationFolder_WarnOnFailureFalse";
            var originalDirectory = $"{_testFolder}\\{testScenarioName}_Original";
            var wrongDirectory = $"{_testFolder}\\{testScenarioName}_WrongFolder.cs";

            FSTestsFixture.CreateFolders(_testFolder, new List<string> { $"{testScenarioName}_Original", $"{testScenarioName}_Renamed" });
            FSTestsFixture.CreateFiles(_testFolder, new List<string> { $"{testScenarioName}_WrongFolder.cs" });

            _logDate = DateTime.Now;
            Fs.SafeRenameDirectory(originalDirectory, wrongDirectory, false);

            Assert.False(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, $"{testScenarioName}_Original {ErrorMessage}"));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Testing purposes only")]
        public void Dispose()
        {
            FSTestsFixture.DeleteTempFolderForTest(_testFolder);
        }
    }
}
