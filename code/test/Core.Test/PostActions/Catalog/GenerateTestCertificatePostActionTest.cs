using Microsoft.Templates.Core.PostActions.Catalog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestArtifacts;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class GenerateTestCertificatePostActionTest
    {
        [Fact]
        public void Execute_Ok()
        {
            var projectName = "test";

            var fakeShell = new FakeGenShell(projectName, @".\TestData\tmp");
            Directory.CreateDirectory(fakeShell.ProjectPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj"), Path.Combine(fakeShell.ProjectPath, "Test.csproj"), true);

            var postAction =  new GenerateTestCertificatePostAction(fakeShell, "TestUser");
            postAction.Execute();
            var certFilePath = Path.Combine(fakeShell.ProjectPath, $"{projectName}_TemporaryKey.pfx");

            Assert.True(File.Exists(certFilePath));

            File.Delete(certFilePath);

        }
    }
}
