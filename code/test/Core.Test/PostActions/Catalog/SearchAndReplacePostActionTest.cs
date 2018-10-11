// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("ExecutionSet", "Minimum")]
    public class SearchAndReplacePostActionTest
    {
        [Fact]
        public void Execute_Success()
        {
            var templateName = "Test";
            var sourceFile = Path.GetFullPath(@".\TestData\temp\Source.cs");
            var mergeFile = Path.GetFullPath(@".\TestData\temp\Source_searchreplace.cs");
            var expected = File.ReadAllText(@".\TestData\SearchReplace\Source_expected.cs").Replace("\r\n", string.Empty).Replace("\n", string.Empty);
            var path = Path.GetFullPath(@".\TestData\temp");

            Directory.CreateDirectory(path);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\SearchReplace\\Source.cs"), sourceFile, true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\SearchReplace\\Source_searchreplace.cs"), mergeFile, true);

            var mergePostAction = new SearchAndReplacePostAction(templateName, new MergeConfiguration(mergeFile, true));
            mergePostAction.Execute();

            var result = File.ReadAllText(sourceFile);

            Directory.Delete(path, true);

            Assert.Equal(expected, result.Replace("\r\n", string.Empty).Replace("\n", string.Empty));
        }

        [Fact]
        public void Execute_FileNotFound_Error()
        {
            var templateName = "Test";
            var mergeFile = Path.GetFullPath(@".\TestData\SearchReplace\NoSource_searchreplace.cs");

            var mergePostAction = new SearchAndReplacePostAction(templateName, new MergeConfiguration(mergeFile, true));

            Exception ex = Assert.Throws<Exception>(() => mergePostAction.Execute());

            Assert.Equal($"Error executing '{typeof(SearchAndReplacePostAction)}'. Related template: {templateName}.", ex.Message);
            Assert.Equal(typeof(FileNotFoundException), ex.InnerException.GetType());
            Assert.Equal($"There is no merge target for file '{mergeFile}'. Related Template: '{templateName}'.", ex.InnerException.Message);
        }

        [Fact]
        public void Execute_FileNotFound_NoError()
        {
            var templateName = "Test";
            var mergeFile = Path.GetFullPath(@".\TestData\temp\NoSource_searchreplace.cs");
            var path = Path.GetFullPath(@".\TestData\temp");

            Directory.CreateDirectory(path);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\SearchReplace\\NoSource_searchreplace.cs"), mergeFile, true);

            GenContext.Current = new FakeContextProvider()
            {
                GenerationOutputPath = path,
                DestinationPath = Path.GetFullPath(@".\Destination\Project"),
            };

            var mergePostAction = new SearchAndReplacePostAction(templateName, new MergeConfiguration(mergeFile, false));

            mergePostAction.Execute();
            var expected = new FailedMergePostActionInfo(
                    "temp\\NoSource.cs",
                    Path.Combine(path, "NoSource_searchreplace.cs"),
                    "temp\\NoSource_failedpostaction.cs",
                    Path.Combine(path, "NoSource_failedpostaction.cs"),
                    $"Could not find file 'temp\\NoSource.cs' to include the following changes. Please review the code blocks to include the changes manually where required in your project. Related Template: '{templateName}'.",
                    MergeFailureType.FileNotFound);

            Directory.Delete(path, true);

            Assert.Collection<FailedMergePostActionInfo>(
                  GenContext.Current.FailedMergePostActions,
                  f1 =>
                  {
                      Assert.Equal(expected.Description, f1.Description);
                      Assert.Equal(expected.FailedFileName, f1.FailedFileName);
                      Assert.Equal(expected.FileName, f1.FileName);
                      Assert.Equal(expected.FilePath, f1.FilePath);
                      Assert.Equal(expected.FailedFilePath, f1.FailedFilePath);
                      Assert.Equal(expected.MergeFailureType, f1.MergeFailureType);
                  });
        }
    }
}
