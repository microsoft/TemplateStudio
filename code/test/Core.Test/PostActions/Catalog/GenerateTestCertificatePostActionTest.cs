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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.Test.Locations;
using Microsoft.Templates.Test.Artifacts;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class GenerateTestCertificatePostActionTest
    {
        [Fact]
        public void Execute_Ok()
        {
            GenContext.Bootstrap(new UnitTestsTemplatesSource(), new FakeGenShell());

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
