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
    public class MergePostActionTest
    {
        [Fact]
        public void Execute_Success()
        {
            var templateName = "Test";
            var sourceFile = Path.GetFullPath(@".\TestData\temp\Source.cs");
            var mergeFile = Path.GetFullPath(@".\TestData\temp\Source_postaction.cs");
            var expected = File.ReadAllText(@".\TestData\Merge\Source_expected.cs");
            var path = Path.GetFullPath(@".\TestData\temp");

            Directory.CreateDirectory(path);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source.cs"), sourceFile, true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_postaction.cs"), mergeFile, true);

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, true));
            mergePostAction.Execute();

            var result = File.ReadAllText(sourceFile);

            Directory.Delete(path, true);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Execute_LineNotFound_Error()
        {
            var templateName = "Test";
            var sourceFile = Path.GetFullPath(@".\TestData\Merge\Source_fail.cs");
            var mergeFile = Path.GetFullPath(@".\TestData\Merge\Source_fail_postaction.cs");

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, true));

            var result = File.ReadAllText(sourceFile);

            Exception ex = Assert.Throws<Exception>(() => mergePostAction.Execute());

            Assert.Equal($"Error executing '{typeof(MergePostAction)}'. Related template: {templateName}.", ex.Message);
            Assert.Equal(typeof(InvalidDataException), ex.InnerException.GetType());
            Assert.Equal($"Line namespace TestData not found in file '{sourceFile}'. Related Template: '{templateName}'.", ex.InnerException.Message);
        }

        [Fact]
        public void Execute_FileNotFound_Error()
        {
            var templateName = "Test";
            var mergeFile = Path.GetFullPath(@".\TestData\Merge\NoSource_postaction.cs");

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, true));

            Exception ex = Assert.Throws<Exception>(() => mergePostAction.Execute());

            Assert.Equal($"Error executing '{typeof(MergePostAction)}'. Related template: {templateName}.", ex.Message);
            Assert.Equal(typeof(FileNotFoundException), ex.InnerException.GetType());
            Assert.Equal($"There is no merge target for file '{mergeFile}'. Related Template: '{templateName}'.", ex.InnerException.Message);
        }

        [Fact]
        public void Execute_LineNotFound_NoError()
        {
            var templateName = "Test";
            var sourceFile = Path.GetFullPath(@".\TestData\temp\Source_fail.cs");
            var mergeFile = Path.GetFullPath(@".\TestData\temp\Source_fail_postaction.cs");
            var outputPath = Path.GetFullPath(@".\TestData\temp");
            var destinationPath = Path.GetFullPath(@".\Destination\Project");

            Directory.CreateDirectory(outputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_fail.cs"), sourceFile, true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_fail_postaction.cs"), mergeFile, true);

            GenContext.Current = new FakeContextProvider()
            {
                OutputPath = outputPath,
                DestinationPath = destinationPath,
                DestinationParentPath = Directory.GetParent(destinationPath).FullName
            };

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, false));

            var result = File.ReadAllText(Path.Combine(outputPath, sourceFile));

            mergePostAction.Execute();
            var expected = new FailedMergePostActionInfo(
                    @"temp\Source_fail.cs",
                    mergeFile,
                    @"temp\Source_fail_failedpostaction.cs",
                    $"Could not find the expected line `namespace TestData` in file 'temp\\Source_fail.cs'. Please merge the content from the postaction file manually. Related Template: '{templateName}'.",
                    MergeFailureType.LineNotFound);

            Directory.Delete(outputPath, true);

            Assert.Collection<FailedMergePostActionInfo>(
                GenContext.Current.FailedMergePostActions,
                f1 =>
                {
                    Assert.Equal(expected.Description, f1.Description);
                    Assert.Equal(expected.FailedFileName, f1.FailedFileName);
                    Assert.Equal(expected.FileName, f1.FileName);
                    Assert.Equal(expected.FilePath, f1.FilePath);
                    Assert.Equal(expected.MergeFailureType, f1.MergeFailureType);
                });
        }

        [Fact]
        public void Execute_FileNotFound_NoError()
        {
            var templateName = "Test";
            var mergeFile = Path.GetFullPath(@".\TestData\temp\NoSource_postaction.cs");
            var outputPath = Path.GetFullPath(@".\TestData\temp");
            var destinationPath = Path.GetFullPath(@".\Destination\Project");

            Directory.CreateDirectory(outputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\NoSource_postaction.cs"), mergeFile, true);

            GenContext.Current = new FakeContextProvider()
            {
                OutputPath = outputPath,
                DestinationPath = destinationPath,
                DestinationParentPath = Directory.GetParent(destinationPath).FullName
            };

            var mergePostAction = new MergePostAction(templateName, new MergeConfiguration(mergeFile, false));

            mergePostAction.Execute();
            var expected = new FailedMergePostActionInfo(
                    "temp\\NoSource.cs",
                    mergeFile,
                    @"temp\NoSource_failedpostaction.cs",
                    $"Could not find file 'temp\\NoSource.cs' to include the following changes. Please review the code blocks to include the changes manually where required in your project. Related Template: '{templateName}'.",
                    MergeFailureType.FileNotFound);

            Directory.Delete(outputPath, true);

            Assert.Collection<FailedMergePostActionInfo>(
                  GenContext.Current.FailedMergePostActions,
                  f1 =>
                  {
                      Assert.Equal(expected.Description, f1.Description);
                      Assert.Equal(expected.FailedFileName, f1.FailedFileName);
                      Assert.Equal(expected.FileName, f1.FileName);
                      Assert.Equal(expected.FilePath, f1.FilePath);
                      Assert.Equal(expected.MergeFailureType, f1.MergeFailureType);
                  });
        }
    }
}
