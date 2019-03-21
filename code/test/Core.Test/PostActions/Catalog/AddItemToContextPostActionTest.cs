// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Fakes;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("ExecutionSet", "Minimum")]
    public class AddItemToContextPostActionTest
    {
        [Fact]
        public void AddItemToContext_Execute()
        {
            var templateName = "Test";
            var relativeFile = @".\Source.cs";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");
            var finalFile = Path.GetFullPath(@".\DestinationPath\Project\Source.cs");

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            List<FakeCreationPath> testPrimaryOutputs = new List<FakeCreationPath>();
            testPrimaryOutputs.Add(new FakeCreationPath() { Path = relativeFile });

            var mergePostAction = new AddItemToContextPostAction(templateName, testPrimaryOutputs, new Dictionary<string, string>(), destPath);
            mergePostAction.Execute();

            Assert.Contains(finalFile, GenContext.Current.ProjectInfo.ProjectItems);
        }

        [Fact]
        public void AddItemToContext_Execute_Replacement()
        {
            var templateName = "Test";
            var relativeFile = @".\Param_ProjectName\Source.cs";
            var destPath = Path.GetFullPath(@".\DestinationPath");
            var finalFile = Path.GetFullPath(@".\DestinationPath\Project\Source.cs");

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            List<FakeCreationPath> testPrimaryOutputs = new List<FakeCreationPath>();
            testPrimaryOutputs.Add(new FakeCreationPath() { Path = relativeFile });

            var genParams = new Dictionary<string, string>();
            genParams.Add("wts.projectName", "Project");

            var mergePostAction = new AddItemToContextPostAction(templateName, testPrimaryOutputs, genParams, destPath);
            mergePostAction.Execute();

            Assert.Contains(finalFile, GenContext.Current.ProjectInfo.ProjectItems);
        }
    }
}
