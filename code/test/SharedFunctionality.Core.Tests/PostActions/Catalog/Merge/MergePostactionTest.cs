// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.PostActions.Catalog.Merge.CodeStyleProviders;
using Microsoft.Templates.Core.Test.TestFakes;
using Microsoft.Templates.Resources;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public class MergePostActionTest
    {
        [Fact]
        public void MergePostAction_Execute_Success()
        {
            var templateName = "Test";
            var sourceFile = Path.GetFullPath(@".\TestData\temp\Source.cs");
            var mergeFile = Path.GetFullPath(@".\TestData\temp\Source_postaction.cs");
            var expected = File.ReadAllText(@".\TestData\Merge\Source_expected.cs").Replace("\r\n", string.Empty).Replace("\n", string.Empty);
            var path = Path.GetFullPath(@".\TestData\temp");

            Directory.CreateDirectory(path);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source.cs"), sourceFile, true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_postaction.cs"), mergeFile, true);

            GenContext.Current = new TestContextProvider()
            {
                GenerationOutputPath = path,
                DestinationPath = path,
            };

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, new CSharpStyleProvider(), true));
            mergePostAction.Execute();

            var result = File.ReadAllText(sourceFile).Replace("\r\n", string.Empty).Replace("\n", string.Empty);

            Directory.Delete(path, true);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void MergePostAction_Execute_LineNotFound_Error()
        {
            var templateName = "Test";
            var sourceFile = Path.GetFullPath(@".\TestData\Merge\Source_fail.cs");
            var mergeFile = Path.GetFullPath(@".\TestData\Merge\Source_fail_postaction.cs");

            GenContext.Current = new TestContextProvider()
            {
                GenerationOutputPath = Environment.CurrentDirectory,
                DestinationPath = Environment.CurrentDirectory,
            };

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, new CSharpStyleProvider(), true));

            var result = File.ReadAllText(sourceFile);

            Exception ex = Assert.Throws<Exception>(() => mergePostAction.Execute());

            Assert.Equal(string.Format(StringRes.PostActionException, typeof(MergePostAction), templateName), ex.Message);
            Assert.Equal(typeof(InvalidDataException), ex.InnerException.GetType());
            Assert.Equal(string.Format(StringRes.MergeLineNotFoundExceptionMessage, "namespace TestData", sourceFile, templateName), ex.InnerException.Message);
        }

        [Fact]
        public void Execute_FileNotFound_ErrorAsync()
        {
            var templateName = "Test";
            var mergeFile = Path.GetFullPath(@".\TestData\Merge\NoSource_postaction.cs");

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, new CSharpStyleProvider(), true));

            Exception ex = Assert.Throws<Exception>(() => mergePostAction.Execute());

            Assert.Equal(string.Format(StringRes.PostActionException, typeof(MergePostAction), templateName), ex.Message);
            Assert.Equal(typeof(FileNotFoundException), ex.InnerException.GetType());
            Assert.Equal(string.Format(StringRes.MergeFileNotFoundExceptionMessage, mergeFile, templateName), ex.InnerException.Message);
        }

        [Fact(Skip = "See issue #4421")]
        public void MergePostAction_Execute_LineNotFound_NoError()
        {
            var templateName = "Test";
            var sourceFile = Path.GetFullPath(@".\TestData\temp\Source_fail.cs");
            var mergeFile = Path.GetFullPath(@".\TestData\temp\Source_fail_postaction.cs");
            var outputPath = Path.GetFullPath(@".\TestData\temp");
            var destinationPath = Path.GetFullPath(@".\Destination\Project");

            Directory.CreateDirectory(outputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_fail.cs"), sourceFile, true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_fail_postaction.cs"), mergeFile, true);

            GenContext.Current = new TestContextProvider()
            {
                GenerationOutputPath = outputPath,
                DestinationPath = destinationPath,
            };

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, new CSharpStyleProvider(), false));

            var result = File.ReadAllText(Path.Combine(outputPath, sourceFile));

            mergePostAction.Execute();
            var expected = new FailedMergePostActionInfo(
                    "temp\\Source_fail.cs",
                    Path.Combine(outputPath, "Source_fail_postaction.cs"),
                    "temp\\Source_fail_failedpostaction.cs",
                    Path.Combine(outputPath, "Source_fail_failedpostaction.cs"),
                    string.Format(StringRes.FailedMergePostActionLineNotFound, "namespace TestData", "temp\\Source_fail.cs", templateName),
                    MergeFailureType.LineNotFound);

            Directory.Delete(outputPath, true);

            Assert.Collection<FailedMergePostActionInfo>(
                GenContext.Current.FailedMergePostActions,
                f1 =>
                {
                    Assert.Equal(expected.Description, f1.Description);
                    Assert.Equal(expected.FailedFileName, f1.FailedFileName);
                    Assert.Equal(expected.FailedFilePath, f1.FailedFilePath);
                    Assert.Equal(expected.FileName, f1.FileName);
                    Assert.Equal(expected.FilePath, f1.FilePath);
                    Assert.Equal(expected.MergeFailureType, f1.MergeFailureType);
                });
        }

        [Fact(Skip = "See issue #4421")]
        public void MergePostAction_Execute_FileNotFound_NoError()
        {
            var templateName = "Test";
            var mergeFile = Path.GetFullPath(@".\TestData\temp\NoSource_postaction.cs");
            var outputPath = Path.GetFullPath(@".\TestData\temp");
            var destinationPath = Path.GetFullPath(@".\Destination\Project");

            Directory.CreateDirectory(outputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\NoSource_postaction.cs"), mergeFile, true);

            GenContext.Current = new TestContextProvider()
            {
                GenerationOutputPath = outputPath,
                DestinationPath = destinationPath,
            };

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, new CSharpStyleProvider(), false));

            mergePostAction.Execute();
            var expected = new FailedMergePostActionInfo(
                    "temp\\NoSource.cs",
                    Path.Combine(outputPath, "NoSource_postaction.cs"),
                    "temp\\NoSource_failedpostaction.cs",
                    Path.Combine(outputPath, "NoSource_failedpostaction.cs"),
                    string.Format(StringRes.FailedMergePostActionFileNotFound, "temp\\NoSource.cs", templateName),
                    MergeFailureType.FileNotFound);

            Directory.Delete(outputPath, true);

            Assert.Collection<FailedMergePostActionInfo>(
                  GenContext.Current.FailedMergePostActions,
                  f1 =>
                  {
                      Assert.Equal(expected.Description, f1.Description);
                      Assert.Equal(expected.FailedFileName, f1.FailedFileName);
                      Assert.Equal(expected.FailedFilePath, f1.FailedFilePath);
                      Assert.Equal(expected.FileName, f1.FileName);
                      Assert.Equal(expected.FilePath, f1.FilePath);
                      Assert.Equal(expected.MergeFailureType, f1.MergeFailureType);
                  });
        }
    }
}
