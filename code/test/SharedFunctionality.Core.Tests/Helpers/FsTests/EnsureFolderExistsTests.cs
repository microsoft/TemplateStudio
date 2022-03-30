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
    public class EnsureFolderExistsTests : IDisposable
    {
        private readonly FSTestsFixture _fixture;
        private readonly string _testFolder;

        private DateTime _logDate;

        private const string ErrorMessage = "Error creating folder";
        private const string ErrorLevel = "Warning";

        public EnsureFolderExistsTests(FSTestsFixture fixture)
        {
            _fixture = fixture;
            Thread.CurrentThread.CurrentUICulture = fixture.CultureInfo;
            _testFolder = _fixture.CreateTempFolderForTest("EnsureFolderExists");
        }

        [Fact]
        public void EnsureFolderExists_DirectoryDoesNotExists_ShouldCreateDirectory()
        {
            var testScenarioName = "DirectoryDoesNotExists";
            var directoryToCreate = $"{_testFolder}\\{testScenarioName}";

            var directoryExistsAtStart = Directory.Exists(directoryToCreate);
            var totalOriginalDirectories = Directory.GetParent(directoryToCreate).GetDirectories().Length;

            Fs.EnsureFolderExists(directoryToCreate);

            var totalNewDirectories = Directory.GetParent(directoryToCreate).GetDirectories().Length;

            Assert.True(totalNewDirectories > totalOriginalDirectories);
            Assert.False(directoryExistsAtStart);
            Assert.True(Directory.Exists(directoryToCreate));
        }

        [Fact]
        public void EnsureFolderExists_DirectoryAlreadyExists_ShouldNotCreateDirectory()
        {
            var testScenarioName = "DirectoryAlreadyExists";
            var directoryToCreate = $"{_testFolder}\\{testScenarioName}";

            FSTestsFixture.CreateFolders(_testFolder, new List<string> { testScenarioName });

            var directoryExistsAtStart = Directory.Exists(directoryToCreate);
            var totalOriginalDirectories = Directory.GetParent(directoryToCreate).GetDirectories().Length;

            Fs.EnsureFolderExists(directoryToCreate);

            var totalNewDirectories = Directory.GetParent(directoryToCreate).GetDirectories().Length;

            Assert.True(totalOriginalDirectories == totalNewDirectories);
            Assert.True(directoryExistsAtStart);
            Assert.True(Directory.Exists(directoryToCreate));
        }

        [Fact(Skip = "See issue #4421")]
        public void EnsureFolderExists_ErrorCreatingDirectory_ShouldLogException()
        {
            // To force an error creating a Directory
            // we create a file with the name of the folder that we want to create
            var testScenarioName = "ErrorCreatingDirectory";
            var wrongDirectoryToCreate = $"{testScenarioName}\\{testScenarioName}.cs";
            FSTestsFixture.CreateFolders(_testFolder, new List<string> { testScenarioName });
            FSTestsFixture.CreateFiles(_testFolder, new List<string> { wrongDirectoryToCreate });

            _logDate = DateTime.Now;
            Fs.EnsureFolderExists($"{_testFolder}\\{wrongDirectoryToCreate}");

            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Testing purposes only")]
        public void Dispose()
        {
            FSTestsFixture.DeleteTempFolderForTest(_testFolder);
        }
    }
}
