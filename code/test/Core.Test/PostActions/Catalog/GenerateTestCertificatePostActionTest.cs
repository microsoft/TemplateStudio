using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Test.Artifacts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class GenerateTestCertificatePostActionTest
    {
        [Fact]
        public void Execute_Ok()
        {
            var projectName = "test";

            GenShell.Initialize(new FakeGenShell());
            GenShell.Current.ContextInfo = GenSolution.Create(projectName, @".\TestData\tmp");

            Directory.CreateDirectory(GenShell.Current.ContextInfo.OutputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj"), Path.Combine(GenShell.Current.ContextInfo.OutputPath, "Test.csproj"), true);

            var postAction =  new GenerateTestCertificatePostAction("TestUser");
            postAction.Execute();
            var certFilePath = Path.Combine(GenShell.Current.ContextInfo.OutputPath, $"{projectName}_TemporaryKey.pfx");

            Assert.True(File.Exists(certFilePath));

            File.Delete(certFilePath);

        }
    }
}
