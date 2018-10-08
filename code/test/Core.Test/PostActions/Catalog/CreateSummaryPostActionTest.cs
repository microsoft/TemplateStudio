// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("ExecutionSet", "Minimum")]
    public class CreateSummaryPostActionTest
    {
        [Fact]
        public void Execute_SyncGeneration()
        {
            var outputPath = Path.GetFullPath(@".\temp");
            var destPath = Path.GetFullPath(@".\DestinationPath");
            var expectedFile = Path.GetFullPath(@".\TestData\GenerationSummary_expected.md");

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                OutputPath = outputPath,
                TempGenerationPath = outputPath,
                DestinationParentPath = Directory.GetParent(destPath).FullName,
            };

            Directory.CreateDirectory(outputPath);

            var config = new TempGenerationResult();
            config.SyncGeneration = true;

            // New File
            config.NewFiles.Add("NewFile.cs");

            // Modified
            config.ModifiedFiles.Add("ModifiedFile_Success.cs");
            GenContext.Current.MergeFilesFromProject.Add("ModifiedFile_Success.cs", new List<MergeInfo>() { new MergeInfo() { Format = "CSHARP", PostActionCode = "**MERGECODEPLACEHOLDER**" } });
            config.ModifiedFiles.Add("ModifiedFile_Error.cs");
            GenContext.Current.MergeFilesFromProject.Add("ModifiedFile_Error.cs", new List<MergeInfo>() { new MergeInfo() { Format = "CSHARP", PostActionCode = "**MERGECODEPLACEHOLDER**" } });
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostActionInfo("ModifiedFile_Error.cs", Path.Combine(outputPath, "ModifiedFile_Error.cs"), "ModifiedFile_failedpostaction.cs", "TestDescription", MergeFailureType.FileNotFound));

            // UnModified
            config.UnchangedFiles.Add("UnchangedFile.cs");

            // Conflicting
            config.ConflictingFiles.Add("ConflictFile.cs");

            var mergePostAction = new CreateSummaryPostAction(config);
            mergePostAction.Execute();

            var filePath = Path.Combine(outputPath, "GenerationSummary.md");

            Assert.True(File.Exists(filePath));

            var expected = File.ReadAllText(expectedFile)
                    .Replace("{{projectPath}}", Directory.GetParent(destPath).FullName.Replace(@"\", "/"))
                    .Replace("{{tempPath}}", outputPath)
                    .Replace("\r\n", string.Empty)
                    .Replace("\n", string.Empty);
            Assert.Equal(expected, File.ReadAllText(filePath).Replace("\r\n", string.Empty).Replace("\n", string.Empty));

            Directory.Delete(outputPath, true);
        }

        [Fact]
        public void Execute_NotSyncGeneration()
        {
            var outputPath = Path.GetFullPath(@".\temp");
            var destPath = Path.GetFullPath(@".\DestinationPath");
            var expectedFile = Path.GetFullPath(@".\TestData\Steps to include new item generation_expected.md");

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                OutputPath = outputPath,
                TempGenerationPath = outputPath,
                DestinationParentPath = Directory.GetParent(destPath).FullName,
            };

            Directory.CreateDirectory(outputPath);

            var config = new TempGenerationResult();
            config.SyncGeneration = false;

            // New File
            config.NewFiles.Add("NewFile.cs");

            // Modified
            config.ModifiedFiles.Add("ModifiedFile_Success.cs");
            GenContext.Current.MergeFilesFromProject.Add("ModifiedFile_Success.cs", new List<MergeInfo>() { new MergeInfo() { Format = "CSHARP", PostActionCode = "**MERGECODEPLACEHOLDER**" } });
            config.ModifiedFiles.Add("ModifiedFile_Error.cs");
            GenContext.Current.MergeFilesFromProject.Add("ModifiedFile_Error.cs", new List<MergeInfo>() { new MergeInfo() { Format = "CSHARP", PostActionCode = "**MERGECODEPLACEHOLDER**" } });
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostActionInfo("ModifiedFile_Error.cs", Path.Combine(outputPath, "ModifiedFile_Error.cs"), "ModifiedFile_failedpostaction.cs", "TestDescription", MergeFailureType.FileNotFound));

            // UnModified
            config.UnchangedFiles.Add("UnchangedFile.cs");

            // Conflicting
            config.ConflictingFiles.Add("ConflictFile.cs");

            var mergePostAction = new CreateSummaryPostAction(config);
            mergePostAction.Execute();

            var filePath = Path.Combine(outputPath, "Steps to include new item generation.md");

            Assert.True(File.Exists(filePath));

            var expected = File.ReadAllText(expectedFile)
                    .Replace("{{projectPath}}", Directory.GetParent(destPath).FullName.Replace(@"\", "/"))
                    .Replace("{{tempPath}}", outputPath)
                    .Replace("\r\n", string.Empty)
                    .Replace("\n", string.Empty);
            Assert.Equal(expected, File.ReadAllText(filePath).Replace("\r\n", string.Empty).Replace("\n", string.Empty));

            Directory.Delete(outputPath, true);
        }
    }
}
