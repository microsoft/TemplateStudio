// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.Test.TestFakes;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public sealed class CreateSummaryPostActionTest : IDisposable
    {
        private CultureInfo cultureInfo;

        public CreateSummaryPostActionTest()
        {
            cultureInfo = CultureInfo.CurrentUICulture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentUICulture = cultureInfo;
        }

        [Fact]
        public void CreateSummary_Execute_SyncGeneration()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            var outputPath = Path.GetFullPath(@".\temp\Project");
            var destPath = Path.GetFullPath(@".\DestinationPath");
            var expectedFile = Path.GetFullPath(@".\TestData\GenerationSummary_expected.md");

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = outputPath,
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
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostActionInfo("ModifiedFile_Error.cs", Path.Combine(outputPath, "ModifiedFile_Error.cs"), "ModifiedFile_failedpostaction.cs", Path.Combine(outputPath, "ModifiedFile_failedpostaction.cs"), "TestDescription", MergeFailureType.FileNotFound));

            // UnModified
            config.UnchangedFiles.Add("UnchangedFile.cs");

            // Conflicting
            config.ConflictingFiles.Add("ConflictFile.cs");

            var mergePostAction = new CreateSummaryPostAction(config);
            mergePostAction.Execute();

            var filePath = Path.Combine(Directory.GetParent(outputPath).FullName, "GenerationSummary.md");

            Assert.True(File.Exists(filePath));

            var expected = File.ReadAllText(expectedFile)
                    .Replace("{{projectPath}}", Directory.GetParent(destPath).FullName.Replace(@"\", "/"))
                    .Replace("{{tempPath}}", Directory.GetParent(outputPath).FullName)
                    .Replace("\r\n", string.Empty)
                    .Replace("\n", string.Empty);
            Assert.Equal(expected, File.ReadAllText(filePath).Replace("\r\n", string.Empty).Replace("\n", string.Empty));

            Directory.Delete(Directory.GetParent(outputPath).FullName, true);
        }

        [Fact]
        public void CreateSummary_Execute_NotSyncGeneration()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            var outputPath = Path.GetFullPath(@".\temp\Project");
            var destPath = Path.GetFullPath(@".\DestinationPath");
            var expectedFile = Path.GetFullPath(@".\TestData\Steps to include new item generation_expected.md");

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = outputPath,
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
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostActionInfo("ModifiedFile_Error.cs", Path.Combine(outputPath, "ModifiedFile_Error.cs"), "ModifiedFile_failedpostaction.cs", Path.Combine(outputPath, "ModifiedFile_failedpostaction.cs"), "TestDescription", MergeFailureType.FileNotFound));

            // UnModified
            config.UnchangedFiles.Add("UnchangedFile.cs");

            // Conflicting
            config.ConflictingFiles.Add("ConflictFile.cs");

            var mergePostAction = new CreateSummaryPostAction(config);
            mergePostAction.Execute();

            var filePath = Path.Combine(Directory.GetParent(outputPath).FullName, "Steps to include new item generation.md");

            Assert.True(File.Exists(filePath));

            var expected = File.ReadAllText(expectedFile)
                    .Replace("{{projectPath}}", Directory.GetParent(destPath).FullName.Replace(@"\", "/"))
                    .Replace("{{tempPath}}", Directory.GetParent(outputPath).FullName)
                    .Replace("\r\n", string.Empty)
                    .Replace("\n", string.Empty);
            Assert.Equal(expected, File.ReadAllText(filePath).Replace("\r\n", string.Empty).Replace("\n", string.Empty));

            Directory.Delete(Directory.GetParent(outputPath).FullName, true);
        }
    }
}
