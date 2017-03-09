using Microsoft.Templates.Core.Gen;
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
            GenContext.Bootstrap(null, new FakeGenShell());

            var projectName = "test";

            using (var context = GenContext.CreateNew(projectName, @".\TestData\tmp"))
            {
                Directory.CreateDirectory(GenContext.Current.OutputPath);
                File.Copy(Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj"), Path.Combine(GenContext.Current.OutputPath, "Test.csproj"), true);

                var postAction = new GenerateTestCertificatePostAction("TestUser");
                postAction.Execute();
                var certFilePath = Path.Combine(GenContext.Current.OutputPath, $"{projectName}_TemporaryKey.pfx");

                Assert.True(File.Exists(certFilePath));

                File.Delete(certFilePath);
            }
        }
    }
}
