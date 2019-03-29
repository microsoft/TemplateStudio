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
    public class AddNugetReferenceToContextPostActionTest
    {
        [Fact]
        public void AddNugetReferenceToContext_Execute()
        {
            var templateName = "Test";
            var projectName = "TestProject";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            var nugetReference = new NugetReference()
            {
                Project = Path.Combine(destPath, projectName),
                PackageId = "TestPackage",
                Version = "1.0.0",
            };

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddNugetReferenceToContextPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "packageId", nugetReference.PackageId },
                    { "version", nugetReference.Version },
                    { "projectPath", projectName },
                });

            var mergePostAction = new AddNugetReferenceToContextPostAction(templateName, postAction, new Dictionary<string, string>(), destPath);
            mergePostAction.Execute();

            Assert.Equal(nugetReference, GenContext.Current.ProjectInfo.NugetReferences[0]);
        }

        [Fact]
        public void AddNugetReferenceToContext_Execute_AlreadyThere()
        {
            var templateName = "Test";
            var projectName = "TestProject";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            var nugetReference = new NugetReference()
            {
                Project = Path.Combine(destPath, projectName),
                PackageId = "TestPackage",
                Version = "1.0.0",
            };

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            GenContext.Current.ProjectInfo.NugetReferences.Add(nugetReference);

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddNugetReferenceToContextPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "packageId", nugetReference.PackageId },
                    { "version", nugetReference.Version },
                    { "projectPath", projectName },
                });

            var mergePostAction = new AddNugetReferenceToContextPostAction(templateName, postAction, new Dictionary<string, string>(), destPath);
            mergePostAction.Execute();

            Assert.Equal(nugetReference, GenContext.Current.ProjectInfo.NugetReferences[0]);
        }

        [Fact]
        public void AddNugetReferenceToContext_Execute_Replacement()
        {
            var templateName = "Test";
            var projectName = "TestProject";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            var nugetReference = new NugetReference()
            {
                Project = Path.Combine(destPath, projectName),
                PackageId = "TestPackage",
                Version = "1.0.0",
            };

            GenContext.Current = new FakeContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddNugetReferenceToContextPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "packageId", nugetReference.PackageId },
                    { "version", nugetReference.Version },
                    { "projectPath", "Param_ProjectName" },
                });

            var genParams = new Dictionary<string, string>();
            genParams.Add("wts.projectName", "TestProject");

            var mergePostAction = new AddNugetReferenceToContextPostAction(templateName, postAction, genParams, destPath);
            mergePostAction.Execute();

            Assert.Equal(nugetReference, GenContext.Current.ProjectInfo.NugetReferences[0]);
        }
    }
}
