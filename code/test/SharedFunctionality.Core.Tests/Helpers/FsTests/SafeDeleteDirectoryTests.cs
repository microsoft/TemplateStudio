// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Test.Helpers.FsTests.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Collection("Unit Test Logs")]
    [Trait("Group", "Minimum")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Testing purposes only")]
    public class SafeDeleteDirectoryTests : IDisposable
    {
        private readonly FSTestsFixture _fixture;
        private readonly string _testFolder;

        public SafeDeleteDirectoryTests(FSTestsFixture fixture)
        {
            _fixture = fixture;
            _testFolder = _fixture.CreateTempFolderForTest("SafeDeleteDirectory");
        }

        [Fact]
        public void SafeDeleteDirectory_DirectoryExists_ShouldDeleteDirectory()
        {
            var testScenarioName = "DirectoryExists";
            var directoryToCreate = $"{_testFolder}\\{testScenarioName}";

            FSTestsFixture.CreateFolders(_testFolder, new List<string>() { testScenarioName });

            var totalOriginalDirectories = Directory.GetParent(directoryToCreate).GetDirectories().Length;

            Fs.SafeDeleteDirectory(directoryToCreate, false);

            var totalNewDirectories = Directory.GetParent(directoryToCreate).GetDirectories().Length;

            var directoryHasBeenDeleted = totalNewDirectories < totalOriginalDirectories;

            Assert.True(directoryHasBeenDeleted);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SafeDeleteDirectory_DirectoryPathIsNullOrEmpty_ShouldNotThrowException(string rootDir)
        {
            Fs.SafeDeleteDirectory(rootDir);
        }

        [Fact]
        public void SafeDeleteDirectory_WrongFolder_ShouldNotThrowException()
        {
            var testScenarioName = "WrongFolder";
            var wrongDirectoryToDelete = $"{_testFolder}\\{testScenarioName}";

            Fs.SafeDeleteDirectory(wrongDirectoryToDelete, false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Testing purposes only")]
        public void Dispose()
        {
            FSTestsFixture.DeleteTempFolderForTest(_testFolder);
        }
    }
}
