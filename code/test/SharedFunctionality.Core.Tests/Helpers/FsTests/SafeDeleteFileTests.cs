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
    public class SafeDeleteFileTests : IDisposable
    {
        private readonly FSTestsFixture _fixture;
        private readonly string _testFolder;

        private DateTime _logDate;
        private const string ErrorMessage = "can't be deleted";
        private const string ErrorLevel = "Warning";

        public SafeDeleteFileTests(FSTestsFixture fixture)
        {
            _fixture = fixture;
            Thread.CurrentThread.CurrentUICulture = fixture.CultureInfo;
            _testFolder = _fixture.CreateTempFolderForTest("SafeDeleteFile");
        }

        [Fact]
        public void SafeDeleteFile_ExistingFile_ShouldHaveDeletedFile()
        {
            var testScenarioName = "ExistingFile";
            var directoryToCreate = $"{_testFolder}\\{testScenarioName}";
            var fileToDelete = $"{directoryToCreate}\\{testScenarioName}.csproj";

            FSTestsFixture.CreateFolders(_testFolder, new List<string>() { testScenarioName });
            FSTestsFixture.CreateFiles(_testFolder, new List<string>() { $"{testScenarioName}\\{testScenarioName}.csproj" });
            var fileExistsBefore = File.Exists(fileToDelete);
            Fs.SafeDeleteFile(fileToDelete);

            Assert.True(fileExistsBefore);
            Assert.False(File.Exists(fileToDelete));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SafeDeleteFile_PathNotFound_ShouldNotThrowException(string filePath)
        {
            Fs.SafeDeleteFile(filePath);
        }

        [Fact]
        public void SafeDeleteFile_NoPermissions_ShouldHandleException()
        {
            var testScenarioName = "NoPermissions";
            var directoryToCreate = $"{_testFolder}\\{testScenarioName}";
            var fileToDelete = $"{directoryToCreate}\\{testScenarioName}.csproj";
            try
            {
                FSTestsFixture.CreateFolders(_testFolder, new List<string>() { testScenarioName });
                FSTestsFixture.CreateFiles(_testFolder, new List<string>() { $"{testScenarioName}\\{testScenarioName}.csproj" }, true);

                _logDate = DateTime.Now;
                Fs.SafeDeleteFile(fileToDelete);

                Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
            }
            finally
            {
                _ = new FileInfo(fileToDelete)
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
