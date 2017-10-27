// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]

    public class GenerateTestCertificatePostActionTest : IContextProvider
    {
        private TemplatesFixture _fixture;

        public GenerateTestCertificatePostActionTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
        }

        public string ProjectName { get; set; }

        public string OutputPath { get; set; }

        public string ProjectPath { get; set; }

        public List<string> ProjectItems { get; } = new List<string>();

        public List<FailedMergePostAction> FailedMergePostActions { get; } = new List<FailedMergePostAction>();

        public Dictionary<string, List<MergeInfo>> MergeFilesFromProject { get; } = new Dictionary<string, List<MergeInfo>>();

        public List<string> FilesToOpen { get; } = new List<string>();

        public Dictionary<ProjectMetricsEnum, double> ProjectMetrics { get; } = new Dictionary<ProjectMetricsEnum, double>();

        [Fact]
        public void Execute_Ok_SingleProjectGenConfigs()
        {
            var projectName = "Test";
            string projectFile = $"{projectName}.csproj";

            ProjectPath = @".\TestData\tmp\TestProject";
            OutputPath = @".\TestData\tmp\TestProject";
            ProjectName = projectName;

            GenContext.Current = this;

            Directory.CreateDirectory(GenContext.Current.OutputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\{projectFile}"), Path.Combine(GenContext.Current.OutputPath, projectFile), true);

            Dictionary<string, string> testArgs = new Dictionary<string, string>();
            testArgs.Add("files", "0");
            List<FakeCreationPath> testPrimaryOutputs = new List<FakeCreationPath>();
            testPrimaryOutputs.Add(new FakeCreationPath() { Path = projectFile });
            IPostAction templateDefinedPostAction = new FakeTemplateDefinedPostAction(GenerateTestCertificatePostAction.Id, testArgs, true);

            var postAction = new GenerateTestCertificatePostAction("TestUser", templateDefinedPostAction, testPrimaryOutputs as IReadOnlyList<ICreationPath>);

            postAction.Execute();

            var expectedCertFilePath = Path.Combine(GenContext.Current.OutputPath, $"{projectName}_TemporaryKey.pfx");

            Assert.True(File.Exists(expectedCertFilePath));

            Fs.SafeDeleteDirectory(OutputPath);
        }

        [Fact]
        public void Execute_Ok_MultipleProjectGenConfig()
        {
            var projectName = "Test";
            string projectFile = $@"TestProject\{projectName}.csproj";

            ProjectPath = @".\TestData\tmp\TestProject";
            OutputPath = @".\TestData\tmp\";
            ProjectName = projectName;

            GenContext.Current = this;

            Directory.CreateDirectory(GenContext.Current.ProjectPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, $"TestData\\{projectFile}"), Path.Combine(GenContext.Current.OutputPath, projectFile), true);

            Dictionary<string, string> testArgs = new Dictionary<string, string>();
            testArgs.Add("files", "0");
            List<FakeCreationPath> testPrimaryOutputs = new List<FakeCreationPath>();
            testPrimaryOutputs.Add(new FakeCreationPath() { Path = projectFile });
            IPostAction templateDefinedPostAction = new FakeTemplateDefinedPostAction(GenerateTestCertificatePostAction.Id, testArgs, true);

            var postAction = new GenerateTestCertificatePostAction("TestUser", templateDefinedPostAction, testPrimaryOutputs as IReadOnlyList<ICreationPath>);

            postAction.Execute();

            var expectedCertFilePath = Path.Combine(GenContext.Current.OutputPath, $"{projectName}_TemporaryKey.pfx");

            Assert.True(File.Exists(expectedCertFilePath));

            Fs.SafeDeleteDirectory(@".\TestData\tmp");
        }

        [Fact]
        public void BadInstantiation_ContinueOnError()
        {
            Dictionary<string, string> testArgs = new Dictionary<string, string>();
            testArgs.Add("myArg", "myValue");
            IPostAction inventedIdPostAction = new FakeTemplateDefinedPostAction(Guid.NewGuid(), testArgs, true);

            var postAction = new GenerateTestCertificatePostAction("TestUser", inventedIdPostAction, null);

            Assert.True(postAction.ContinueOnError);
            Assert.NotEqual(inventedIdPostAction.ActionId, GenerateTestCertificatePostAction.Id);
            Assert.False(postAction.Intialized);
            Assert.Null(postAction.Args);
        }

        [Fact]
        public void BadInstantiation_NoContinueOnError()
        {
            Assert.Throws<Exception>(() =>
               {
                   Dictionary<string, string> testArgs = new Dictionary<string, string>();
                   testArgs.Add("myArg", "myValue");
                   IPostAction inventedIdPostAction = new FakeTemplateDefinedPostAction(Guid.NewGuid(), testArgs, false);
                   var postAction = new GenerateTestCertificatePostAction("TestUser", inventedIdPostAction, null);
               });
        }
    }
}
