// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Test.Helpers.FsTests.Helpers;
using Xunit;

namespace Microsoft.Templates.Core.Test.Helpers.FsTests
{
    [Collection("Unit Test Logs")]
    [Trait("Group", "Minimum")]
    public class GetExistingFolderNamesTests
    {
        private readonly FSTestsFixture _fixture;
        private readonly string _testFolder;

        public GetExistingFolderNamesTests(FSTestsFixture fixture)
        {
            _fixture = fixture;
            _testFolder = _fixture.CreateTempFolderForTest("GetExistingFolderNames");
        }

        [Fact]
        public void GetExistingFolderNames_RootExists_ShouldReturnAllExpectedFolderNamesInAlphabeticalOrder()
        {
            var testScenarioName = "RootExists";
            var directoryExists = $"{_testFolder}\\{testScenarioName}";

            FSTestsFixture.CreateFolders(_testFolder, new List<string> { testScenarioName, $"{testScenarioName}\\One", $"{testScenarioName}\\Two", $"{testScenarioName}\\Three" });

            var expected = new List<string>() { "One", "Three", "Two" };

            var actual = Fs.GetExistingFolderNames(directoryExists);

            Assert.Equal(3, actual.Count());
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetExistingFolderNames_RootDirectoryEmptyOrNull_ShouldReturnEmptyList(string rootDirectory)
        {
            var actual = Fs.GetExistingFolderNames(rootDirectory);

            Assert.Empty(actual);
        }

        [Fact]
        public void GetExistingFolderNames_RootDirectoryDoesNotExist_ShouldReturnEmptyList()
        {
            var testScenarioName = "RootDirectoryDoesNotExist";
            var directoryDoesNotExist = $"{_testFolder}\\{testScenarioName}";

            var actual = Fs.GetExistingFolderNames(directoryDoesNotExist);

            Assert.Empty(actual);
        }
    }
}
