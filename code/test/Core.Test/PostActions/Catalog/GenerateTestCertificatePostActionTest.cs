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
using System.IO;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.Test.Locations;
using Microsoft.Templates.Test.Artifacts;

using Xunit;
using System.Collections.Generic;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class GenerateTestCertificatePostActionTest : IContextProvider
    {
        public string ProjectName { get; set; }
        public string OutputPath { get; set; }

        public List<string> ProjectItems => throw new NotImplementedException();

        [Fact]
        public void Execute_Ok()
        {
            GenContext.Bootstrap(new UnitTestsTemplatesSource(), new FakeGenShell());

            var projectName = "test";

            ProjectName = projectName;
            OutputPath = @".\TestData\tmp";

            GenContext.Current = this;


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
