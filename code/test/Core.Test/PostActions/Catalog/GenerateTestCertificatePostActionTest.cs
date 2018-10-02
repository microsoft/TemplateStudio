// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
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
        public void Execute_Ok_SingleProjectGenConfigs()
        {
            var projectName = "Test";
            var projectFile = $"{projectName}.csproj";
            var destinationPath = @".\TestData\temp\TestProject";
            var generationOutputPath = @".\TestData\temp\TestProject";

            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.CSharp), ProgrammingLanguages.CSharp);

            GenContext.Current = new FakeContextProvider
            {
                DestinationPath = destinationPath,
                GenerationOutputPath = generationOutputPath
            };

            Directory.CreateDirectory(generationOutputPath);
            var sourceFile = Path.Combine(Environment.CurrentDirectory, $"TestData\\TestProject\\{projectFile}");
            var destFile = Path.Combine(generationOutputPath, projectFile);
            File.Copy(sourceFile, destFile, true);

            var testArgs = new Dictionary<string, string>
            {
                { "files", "0" }
            };

            var testPrimaryOutputs = new List<FakeCreationPath>()
            {
                new FakeCreationPath()
                {
                    Path = projectFile
                }
            };

            var templateDefinedPostAction = new FakeTemplateDefinedPostAction(GenerateTestCertificatePostAction.Id, testArgs, true);
            var postAction = new GenerateTestCertificatePostAction("TestTemplate", "TestUser", templateDefinedPostAction, testPrimaryOutputs as IReadOnlyList<ICreationPath>, new Dictionary<string, string>(), destinationPath);
            postAction.Execute();

            var expectedCertFilePath = Path.Combine(generationOutputPath, $"{projectName}_TemporaryKey.pfx");
            Assert.True(File.Exists(expectedCertFilePath));

            Fs.SafeDeleteDirectory(generationOutputPath);
        }

        [Fact]
        public void Execute_Ok_MultipleProjectGenConfig()
        {
            var projectName = "Test";
            var projectFile = $@"TestProject\{projectName}.csproj";
            var destinationPath = @".\TestData\temp";
            var generationOutputPath = @".\TestData\temp\";

            GenContext.Bootstrap(new LocalTemplatesSource(), new FakeGenShell(Platforms.Uwp, ProgrammingLanguages.CSharp), ProgrammingLanguages.CSharp);

            GenContext.Current = new FakeContextProvider
            {
                DestinationPath = destinationPath,
                GenerationOutputPath = generationOutputPath
            };

            Directory.CreateDirectory(Path.Combine(destinationPath, "TestProject"));
            var sourceFile = Path.Combine(Environment.CurrentDirectory, $"TestData\\{projectFile}");
            var destFile = Path.Combine(generationOutputPath, projectFile);
            File.Copy(sourceFile, destFile, true);

            var testArgs = new Dictionary<string, string>()
            {
                { "files", "0" }
            };

            var testPrimaryOutputs = new List<FakeCreationPath>()
            {
                new FakeCreationPath() { Path = projectFile }
            };

            var templateDefinedPostAction = new FakeTemplateDefinedPostAction(GenerateTestCertificatePostAction.Id, testArgs, true);
            var postAction = new GenerateTestCertificatePostAction("TestTemplate", "TestUser", templateDefinedPostAction, testPrimaryOutputs, new Dictionary<string, string>(), destinationPath);
            postAction.Execute();

            var expectedCertFilePath = Path.Combine(destinationPath, "TestProject", $"{projectName}_TemporaryKey.pfx");
            Assert.True(File.Exists(expectedCertFilePath));
            Fs.SafeDeleteDirectory(destinationPath);
        }

        [Fact]
        public void BadInstantiation_ContinueOnError()
        {
            var testArgs = new Dictionary<string, string>() { { "myArg", "myValue" } };
            var inventedIdPostAction = new FakeTemplateDefinedPostAction(Guid.NewGuid(), testArgs, true);
            var postAction = new GenerateTestCertificatePostAction("TestTemplate", "TestUser", inventedIdPostAction, null, new Dictionary<string, string>(), string.Empty);

            Assert.True(postAction.ContinueOnError);
            Assert.NotEqual(inventedIdPostAction.ActionId, GenerateTestCertificatePostAction.Id);
            Assert.Null(postAction.Args);
        }

        [Fact]
        public void BadInstantiation_NoContinueOnError()
        {
            Assert.Throws<Exception>(() =>
               {
                   var testArgs = new Dictionary<string, string>() { { "myArg", "myValue" } };
                   var inventedIdPostAction = new FakeTemplateDefinedPostAction(Guid.NewGuid(), testArgs, false);
                   var postAction = new GenerateTestCertificatePostAction("TestTemplate", "TestUser", inventedIdPostAction, null, new Dictionary<string, string>(), string.Empty);
               });
        }
    }
}
