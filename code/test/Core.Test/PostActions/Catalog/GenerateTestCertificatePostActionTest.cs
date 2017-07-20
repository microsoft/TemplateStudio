// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Collection("Unit Test Templates")]
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


        [Fact, Trait("Type", "ProjectGeneration")]
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
