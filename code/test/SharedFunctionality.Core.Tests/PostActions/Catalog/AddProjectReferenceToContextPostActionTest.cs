// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.Test.TestFakes;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Trait("Group", "Minimum")]
    public class AddProjectReferenceToContextPostActionTest
    {
        [Fact]
        public void AddProjectReferenceToContext_Execute()
        {
            var templateName = "Test";
            var projectName = "MyProject";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");
            var projectToAdd = @".\TestProject.csproj";

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            List<FakeCreationPath> testPrimaryOutputs = new List<FakeCreationPath>();
            testPrimaryOutputs.Add(new FakeCreationPath() { Path = projectToAdd });

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddProjectReferencesToContextPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "fileIndex", "0" },
                    { "projectPath", projectName },
                });

            var mergePostAction = new AddProjectReferencesToContextPostAction(templateName, postAction, testPrimaryOutputs, new Dictionary<string, string>(), destPath);
            mergePostAction.Execute();

            Assert.Contains(GenContext.Current.ProjectInfo.ProjectReferences, p => p.Project == Path.Combine(destPath, projectName));
            Assert.Contains(GenContext.Current.ProjectInfo.ProjectReferences, p => p.Project == Path.Combine(destPath, projectName) && p.ReferencedProject == Path.GetFullPath(Path.Combine(destPath, projectToAdd)));
        }

        [Fact]
        public void AddSdkReferenceToContext_Execute_AlreadyThere()
        {
            var templateName = "Test";
            var projectName = "MyProject";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");
            var projectToAdd = @".\TestProject.csproj";

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            var projectReference = new ProjectReference()
            {
                Project = Path.Combine(destPath, projectName),
                ReferencedProject = "FirstReference",
            };

            GenContext.Current.ProjectInfo.ProjectReferences.Add(projectReference);

            List<FakeCreationPath> testPrimaryOutputs = new List<FakeCreationPath>();
            testPrimaryOutputs.Add(new FakeCreationPath() { Path = projectToAdd });

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddProjectReferencesToContextPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "fileIndex", "0" },
                    { "projectPath", projectName },
                });

            var mergePostAction = new AddProjectReferencesToContextPostAction(templateName, postAction, testPrimaryOutputs, new Dictionary<string, string>(), destPath);
            mergePostAction.Execute();

            Assert.Contains(GenContext.Current.ProjectInfo.ProjectReferences, p => p.Project == Path.Combine(destPath, projectName));
            Assert.Contains(GenContext.Current.ProjectInfo.ProjectReferences, p => p.Project == Path.Combine(destPath, projectName) && p.ReferencedProject == Path.GetFullPath(Path.Combine(destPath, projectToAdd)));
        }
    }
}
