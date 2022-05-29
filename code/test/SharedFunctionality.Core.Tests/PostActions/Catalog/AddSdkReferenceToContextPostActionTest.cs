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
    public class AddSdkReferenceToContextPostActionTest
    {
        [Fact]
        public void AddSdkReferenceToContext_Execute()
        {
            var templateName = "Test";
            var projectName = "TestProject";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            var sdkReference = new SdkReference()
            {
                Project = Path.Combine(destPath, projectName),
                Name = "TestName",
                Sdk = "Test, version=1.0.0",
            };

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddSdkReferencesToContextPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "name", sdkReference.Name },
                    { "sdk", sdkReference.Sdk },
                    { "projectPath", projectName },
                });

            var mergePostAction = new AddSdkReferencesToContextPostAction(templateName, postAction, new Dictionary<string, string>(), destPath);
            mergePostAction.Execute();

            Assert.Equal(sdkReference, GenContext.Current.ProjectInfo.SdkReferences[0]);
        }

        [Fact(Skip = "See issue #4421")]
        public void AddSdkReferenceToContext_Execute_AlreadyThere()
        {
            var templateName = "Test";
            var projectName = "TestProject";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            var sdkReference = new SdkReference()
            {
                Project = Path.Combine(destPath, projectName),
                Name = "TestName",
                Sdk = "Test, version=1.0.0",
            };

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            GenContext.Current.ProjectInfo.SdkReferences.Add(sdkReference);

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddSdkReferencesToContextPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "name", sdkReference.Name },
                    { "sdk", sdkReference.Sdk },
                    { "projectPath", projectName },
                });

            var mergePostAction = new AddSdkReferencesToContextPostAction(templateName, postAction, new Dictionary<string, string>(), destPath);
            mergePostAction.Execute();

            Assert.Equal(sdkReference, GenContext.Current.ProjectInfo.SdkReferences[0]);
        }

        [Fact]
        public void AddSdkReferenceToContext_Execute_Replacement()
        {
            var templateName = "Test";
            var projectName = "TestProject";
            var destPath = Path.GetFullPath(@".\DestinationPath\Project");

            var sdkReference = new SdkReference()
            {
                Project = Path.Combine(destPath, projectName),
                Name = "TestName",
                Sdk = "Test, version=1.0.0",
            };

            GenContext.Current = new TestContextProvider()
            {
                DestinationPath = destPath,
                GenerationOutputPath = destPath,
            };

            var postAction = new FakeTemplateDefinedPostAction(
                new Guid(AddSdkReferencesToContextPostAction.Id),
                new Dictionary<string, string>()
                {
                    { "name", sdkReference.Name },
                    { "sdk", sdkReference.Sdk },
                    { "projectPath", projectName },
                });

            var genParams = new Dictionary<string, string>();
            genParams.Add(GenParams.ProjectName, "TestProject");

            var mergePostAction = new AddSdkReferencesToContextPostAction(templateName, postAction, genParams, destPath);
            mergePostAction.Execute();

            Assert.Equal(sdkReference, GenContext.Current.ProjectInfo.SdkReferences[0]);
        }
    }
}
