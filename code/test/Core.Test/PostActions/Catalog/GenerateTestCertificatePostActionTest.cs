// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Fakes;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    [Collection("Unit Test Templates")]
    [Trait("ExecutionSet", "Minimum")]
    public class GenerateTestCertificatePostActionTest
    {
        [Fact]
        public void GenerateTestCertificate_Execute_Ok_SingleProjectGenConfigs()
        {
            var projectName = "Test";
            var projectFile = $"{projectName}.csproj";
            var destinationPath = @".\TestData\temp\TestProject";
            var generationOutputPath = @".\TestData\temp\TestProject";

            GenContext.Bootstrap(new LocalTemplatesSource(null), new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.CSharp), Platforms.Uwp, ProgrammingLanguages.CSharp);

            GenContext.Current = new FakeContextProvider
            {
                DestinationPath = destinationPath,
                GenerationOutputPath = generationOutputPath,
            };

            Directory.CreateDirectory(generationOutputPath);
            var sourceFile = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\{projectFile}");
            var destFile = Path.Combine(generationOutputPath, projectFile);
            File.Copy(sourceFile, destFile, true);

            var testArgs = new Dictionary<string, string>
            {
                { "files", "0" },
            };

            var testPrimaryOutputs = new List<FakeCreationPath>()
            {
                new FakeCreationPath() { Path = projectFile },
            };

            var templateDefinedPostAction = new FakeTemplateDefinedPostAction(new Guid(GenerateTestCertificatePostAction.Id), testArgs, true);
            var postAction = new GenerateTestCertificatePostAction("TestTemplate", "TestUser", templateDefinedPostAction, testPrimaryOutputs as IReadOnlyList<ICreationPath>, new Dictionary<string, string>(), destinationPath);
            postAction.Execute();

            var expectedCertFilePath = Path.Combine(generationOutputPath, $"{projectName}_TemporaryKey.pfx");
            Assert.True(File.Exists(expectedCertFilePath));

            Fs.SafeDeleteDirectory(generationOutputPath);
        }

        [Fact]
        public void GenerateTestCertificate_Execute_Ok_MultipleProjectGenConfig()
        {
            var projectName = "Test";
            var projectFile = $@"TestProject\{projectName}.csproj";
            var destinationPath = @".\TestData\temp";
            var generationOutputPath = @".\TestData\temp\";

            GenContext.Bootstrap(new LocalTemplatesSource(null), new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.CSharp), Platforms.Uwp, ProgrammingLanguages.CSharp);

            GenContext.Current = new FakeContextProvider
            {
                DestinationPath = destinationPath,
                GenerationOutputPath = generationOutputPath,
            };

            Directory.CreateDirectory(Path.Combine(destinationPath, "TestProject"));
            var sourceFile = Path.Combine(Environment.CurrentDirectory, $"TestData\\{projectFile}");
            var destFile = Path.Combine(generationOutputPath, projectFile);
            File.Copy(sourceFile, destFile, true);

            var testArgs = new Dictionary<string, string>()
            {
                { "files", "0" },
            };

            var testPrimaryOutputs = new List<FakeCreationPath>()
            {
                new FakeCreationPath() { Path = projectFile },
            };

            var templateDefinedPostAction = new FakeTemplateDefinedPostAction(new Guid(GenerateTestCertificatePostAction.Id), testArgs, true);
            var postAction = new GenerateTestCertificatePostAction("TestTemplate", "TestUser", templateDefinedPostAction, testPrimaryOutputs, new Dictionary<string, string>(), destinationPath);
            postAction.Execute();

            var expectedCertFilePath = Path.Combine(destinationPath, "TestProject", $"{projectName}_TemporaryKey.pfx");
            Assert.True(File.Exists(expectedCertFilePath));
            Fs.SafeDeleteDirectory(destinationPath);
        }
    }
}
