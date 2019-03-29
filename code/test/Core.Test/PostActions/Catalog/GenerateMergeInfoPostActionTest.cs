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
    public class GenerateMergeInfoPostActionTest
    {
        [Fact]
        public void GenerateMergeInfo_Execute_Success()
        {
            var templateName = "Test";
            var relSourceFilePath = @"Source.cs";
            var mergeFile = Path.GetFullPath(@".\temp\Source_postaction.cs");
            var outputPath = Path.GetFullPath(@".\temp\Project");
            var destinationPath = Path.GetFullPath(@".\DestinationPath\Project");
            var expectedPostactionCode = File.ReadAllText(@".\TestData\Merge\Source_expectedmergeinfo.cs");

            Directory.CreateDirectory(outputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\Merge\\Source_postaction.cs"), mergeFile, true);

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destinationPath,
                GenerationOutputPath = outputPath,
            };

            GenContext.Current.MergeFilesFromProject.Add(relSourceFilePath, new List<MergeInfo>());

            var mergePostAction = new GenerateMergeInfoPostAction(templateName, mergeFile);
            mergePostAction.Execute();

            var expected = new MergeInfo()
            {
                Format = "CSHARP",
                PostActionCode = expectedPostactionCode,
            };

            Directory.Delete(Directory.GetParent(outputPath).FullName, true);

            Assert.Equal(expected.Format, GenContext.Current.MergeFilesFromProject[relSourceFilePath][0].Format);
            Assert.Equal(expected.PostActionCode.Replace("\r\n", string.Empty).Replace("\n", string.Empty), GenContext.Current.MergeFilesFromProject[relSourceFilePath][0].PostActionCode.Replace("\r\n", string.Empty).Replace("\n", string.Empty));
        }
    }
}
