// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.Test.TestFakes;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public class CopyFilesToProjectPostActionTest
    {
        [Fact(Skip = "See issue #4421")]
        public void CopyFilesToProject_Execute_NewFile()
        {
            var tempFile = Path.GetFullPath(@".\temp\Source.cs");
            var path = Path.GetFullPath(@".\temp\Project");
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");
            var finalFile = Path.GetFullPath(@".\DestinationPath\Source.cs");

            Directory.CreateDirectory(path);
            Directory.CreateDirectory(destPath);

            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_postaction.cs"), tempFile, true);

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = path,
            };

            var config = new TempGenerationResult();
            config.NewFiles.Add("Source.cs");

            var mergePostAction = new CopyFilesToProjectPostAction(config);
            mergePostAction.Execute();

            Assert.True(File.Exists(Path.Combine(Directory.GetParent(destPath).FullName, "Source.cs")));

            Directory.Delete(Directory.GetParent(path).FullName, true);
            Directory.Delete(Directory.GetParent(destPath).FullName, true);

            Assert.Contains(finalFile, GenContext.Current.FilesToOpen);
        }

        [Fact(Skip = "See issue #4421")]
        public void CopyFilesToProject_Execute_ModifiedFile()
        {
            var tempFile = Path.GetFullPath(@".\temp\Source.cs");
            var path = Path.GetFullPath(@".\temp\Project");
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");
            var finalFile = Path.GetFullPath(@".\DestinationPath\Source.cs");

            Directory.CreateDirectory(path);
            Directory.CreateDirectory(destPath);

            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_postaction.cs"), tempFile, true);

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = path,
            };

            var config = new TempGenerationResult();
            config.ModifiedFiles.Add("Source.cs");

            var mergePostAction = new CopyFilesToProjectPostAction(config);
            mergePostAction.Execute();

            Assert.True(File.Exists(Path.Combine(Directory.GetParent(destPath).FullName, "Source.cs")));

            Directory.Delete(Directory.GetParent(path).FullName, true);
            Directory.Delete(Directory.GetParent(destPath).FullName, true);

            Assert.DoesNotContain(finalFile, GenContext.Current.FilesToOpen);
        }
    }
}
