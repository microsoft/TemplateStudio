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
    public class EnsureFileEditableTests : IDisposable
    {
        private readonly FSTestsFixture _fixture;
        private readonly string _testFolder;

        private DateTime _logDate;

        private const string ErrorMessage = "Cannot remove readonly protection from file";
        private const string ErrorLevel = "Warning";

        public EnsureFileEditableTests(FSTestsFixture fixture)
        {
            _fixture = fixture;
            Thread.CurrentThread.CurrentUICulture = fixture.CultureInfo;
            _testFolder = _fixture.CreateTempFolderForTest("EnsureFolderExists");
        }

        [Fact]
        public void EnsureFileEditable_FileIsReadOnly_ShouldChangeToReadOnly()
        {
            var testScenarioName = "FileIsReadOnly";
            var fileToEdit = $"{_testFolder}\\{testScenarioName}";
            try
            {
                FSTestsFixture.CreateFiles(_testFolder, new List<string> { testScenarioName }, true);

                Fs.EnsureFileEditable(fileToEdit);
                var newFileInfo = new FileInfo(fileToEdit);

                Assert.False(newFileInfo.IsReadOnly);
            }
            finally
            {
                _ = new FileInfo(fileToEdit)
                {
                    IsReadOnly = false,
                };
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EnsureFileEditable_FilePathNullOrEmpty_ShouldLogError(string filePath)
        {
            _logDate = DateTime.Now;

            Fs.EnsureFileEditable(filePath);

            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void EnsureFileEditable_FileDoesNotExist_ShouldLogError()
        {
            var testScenarioName = "FileDoesNotExist";
            string filePath = $"{_testFolder}\\{testScenarioName}";
            _logDate = DateTime.Now;

            Fs.EnsureFileEditable(filePath);

            Assert.True(_fixture.IsErrorMessageInLogFile(_logDate, ErrorLevel, ErrorMessage));
        }

        [Fact]
        public void EnsureFileEditable_FileIsNotReadOnly_ShouldNotModifyIsReadOnly()
        {
            var testScenarioName = "FileIsNotReadOnly";
            string filePath = $"{_testFolder}\\{testScenarioName}";
            FSTestsFixture.CreateFiles(_testFolder, new List<string> { testScenarioName });

            var originalFileInfo = new FileInfo(filePath);
            Fs.EnsureFileEditable(filePath);
            var newFileInfo = new FileInfo(filePath);

            Assert.False(originalFileInfo.IsReadOnly);
            Assert.False(newFileInfo.IsReadOnly);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Testing purposes only")]
        public void Dispose()
        {
            FSTestsFixture.DeleteTempFolderForTest(_testFolder);
        }
    }
}
