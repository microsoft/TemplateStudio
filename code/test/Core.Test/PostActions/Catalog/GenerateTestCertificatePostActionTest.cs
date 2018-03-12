// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

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
        public void Execute_Ok()
        {
            var projectName = "test";

            ProjectName = projectName;
            ProjectPath = @".\TestData\tmp";

            GenContext.Current = this;

            Directory.CreateDirectory(GenContext.Current.ProjectPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj"), Path.Combine(GenContext.Current.ProjectPath, "Test.csproj"), true);

            var postAction = new GenerateTestCertificatePostAction("TestUser");

            postAction.Execute();

            var certFilePath = Path.Combine(GenContext.Current.ProjectPath, $"{projectName}_TemporaryKey.pfx");

            Assert.True(File.Exists(certFilePath));

            File.Delete(certFilePath);
        }
    }
}
