// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("ExecutionSet", "Minimum")]
    public class GetMergeFilesFromProjectPostActionTest
    {
        [Fact]
        public void GetMergeFilesFromProject_Execute_Postaction()
        {
            var templateName = "Test";
            var relSourceFilePath = @"Source.cs";
            var mergeFile = Path.GetFullPath(@".\temp\Source_postaction.cs");
            var path = Path.GetFullPath(@".\temp\Project");

            Directory.CreateDirectory(path);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_postaction.cs"), mergeFile, true);

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = Path.GetFullPath(@".\DestinationPath\Project"),
                GenerationOutputPath = path,
            };

            var mergePostAction = new GetMergeFilesFromProjectPostAction(templateName, mergeFile);
            mergePostAction.Execute();

            Directory.Delete(Directory.GetParent(path).FullName, true);

            Assert.True(GenContext.Current.MergeFilesFromProject.ContainsKey(relSourceFilePath));
        }

        [Fact]
        public void GetMergeFilesFromProject_Execute_Postaction_FileFound()
        {
            var templateName = "Test";
            var relSourceFilePath = @"Source.cs";
            var sourceFile = Path.GetFullPath(@".\DestinationPath\Source.cs");
            var mergeFile = Path.GetFullPath(@".\temp\Source_postaction.cs");
            var outputPath = Path.GetFullPath(@".\temp\Project");
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            Directory.CreateDirectory(outputPath);
            Directory.CreateDirectory(destPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source.cs"), sourceFile, true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_postaction.cs"), mergeFile, true);

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = outputPath,
            };

            var mergePostAction = new GetMergeFilesFromProjectPostAction(templateName, mergeFile);
            mergePostAction.Execute();

            Directory.Delete(Directory.GetParent(outputPath).FullName, true);
            Directory.Delete(destPath, true);

            Assert.True(GenContext.Current.MergeFilesFromProject.ContainsKey(relSourceFilePath));
        }

        [Fact]
        public void GetMergeFilesFromProject_Execute_Postaction_LocallyAvailable()
        {
            var templateName = "Test";
            var relSourceFilePath = @"Source.cs";
            var sourceFile = Path.GetFullPath(@".\temp\Source.cs");
            var mergeFile = Path.GetFullPath(@".\temp\Source_postaction.cs");
            var outputPath = Path.GetFullPath(@".\temp\Project");
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            Directory.CreateDirectory(outputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source.cs"), sourceFile, true);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_postaction.cs"), mergeFile, true);

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = outputPath,
            };

            var mergePostAction = new GetMergeFilesFromProjectPostAction(templateName, mergeFile);
            mergePostAction.Execute();

            Directory.Delete(Directory.GetParent(outputPath).FullName, true);

            Assert.False(GenContext.Current.MergeFilesFromProject.ContainsKey(relSourceFilePath));
        }

        [Fact]
        public void GetMergeFilesFromProject_Execute_GlobalPostaction()
        {
            var templateName = "Test";
            var relSourceFilePath = @"Source.cs";
            var mergeFile = Path.GetFullPath(@".\temp\Source_gpostaction.cs");
            var outputPath = Path.GetFullPath(@".\temp\Project");
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            Directory.CreateDirectory(outputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_gpostaction.cs"), mergeFile, true);

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = outputPath,
            };

            var mergePostAction = new GetMergeFilesFromProjectPostAction(templateName, mergeFile);
            mergePostAction.Execute();

            Directory.Delete(Directory.GetParent(outputPath).FullName, true);

            Assert.True(GenContext.Current.MergeFilesFromProject.ContainsKey(relSourceFilePath));
        }
    }
}
