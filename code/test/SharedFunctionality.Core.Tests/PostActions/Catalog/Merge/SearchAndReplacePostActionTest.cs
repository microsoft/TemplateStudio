// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;
using Microsoft.Templates.Core.Test.TestFakes;
using Microsoft.Templates.SharedResources;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public class SearchAndReplacePostActionTest
    {
        [Fact(Skip = "See issue #4421")]
        public void SearchAndReplace_Execute_Success()
        {
            var templateName = "Test";
            var sourceFile = Path.GetFullPath(@".\TestData\temp\Source.cs");
            var mergeFile = Path.GetFullPath(@".\TestData\temp\Source_searchreplace.cs");
            var expected = File.ReadAllText(@".\TestData\SearchReplace\Source_expected.cs").Replace("\r\n", string.Empty).Replace("\n", string.Empty);
            var path = Path.GetFullPath(@".\TestData\temp");

            Directory.CreateDirectory(path);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\SearchReplace\\Source.cs"), sourceFile, true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\SearchReplace\\Source_searchreplace.cs"), mergeFile, true);

            GenContext.Current = new TestContextProvider()
            {
                GenerationOutputPath = path,
                DestinationPath = path,
            };

            var mergePostAction = new SearchAndReplacePostAction(templateName, new MergeConfiguration(mergeFile, new CSharpStyleProvider(), true));
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

            var mergePostAction = new SearchAndReplacePostAction(templateName, new MergeConfiguration(mergeFile, new CSharpStyleProvider(), true));

            Exception ex = Assert.Throws<Exception>(() => mergePostAction.Execute());

            Assert.Equal(string.Format(Resources.PostActionException, typeof(SearchAndReplacePostAction), templateName), ex.Message);
            Assert.Equal(typeof(FileNotFoundException), ex.InnerException.GetType());
            Assert.Equal(string.Format(Resources.MergeFileNotFoundExceptionMessage, mergeFile, templateName), ex.InnerException.Message);
        }

        [Fact(Skip = "See issue #4421")]
        public void SearchAndReplace_Execute_FileNotFound_NoError()
        {
            var templateName = "Test";
            var mergeFile = Path.GetFullPath(@".\TestData\temp\NoSource_searchreplace.cs");
            var path = Path.GetFullPath(@".\TestData\temp");

            Directory.CreateDirectory(path);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\SearchReplace\\NoSource_searchreplace.cs"), mergeFile, true);

            GenContext.Current = new TestContextProvider()
            {
                GenerationOutputPath = path,
                DestinationPath = Path.GetFullPath(@".\Destination\Project"),
            };

            var mergePostAction = new SearchAndReplacePostAction(templateName, new MergeConfiguration(mergeFile, new CSharpStyleProvider(), false));

            mergePostAction.Execute();
            var expected = new FailedMergePostActionInfo(
                    "temp\\NoSource.cs",
                    Path.Combine(path, "NoSource_searchreplace.cs"),
                    "temp\\NoSource_failedpostaction.cs",
                    Path.Combine(path, "NoSource_failedpostaction.cs"),
                    string.Format(Resources.FailedMergePostActionFileNotFound, "temp\\NoSource.cs", templateName),
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
