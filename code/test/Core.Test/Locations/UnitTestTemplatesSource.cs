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

using Microsoft.Templates.Core.Locations;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class UnitTestsTemplatesSource : TemplatesSource
    {
        private string LocalVersion = "0.0.0.0";

        public override string Id { get => "UnitTest"; }

        protected override string ObtainMstx()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"C:\code\WindowsTemplateStudio\code\TestCert.pfx", certPass);

            var tempFolder = Path.Combine(GetTempFolder(), SourceFolderName);

            var sourcePath = $@"..\..\TestData\{SourceFolderName}";

            Copy(sourcePath, tempFolder);

            File.WriteAllText(Path.Combine(tempFolder, "version.txt"), LocalVersion);

            return Templatex.PackAndSign(tempFolder, cert);
        }

        protected static void Copy(string sourceFolder, string targetFolder)
        {
            Fs.SafeDeleteDirectory(targetFolder);
            Fs.CopyRecursive(sourceFolder, targetFolder);
        }

        private static SecureString GetTestCertPassword()
        {
            var ss = new SecureString();
            foreach (var c in "pass@word1")
            {
                ss.AppendChar(c);
            }

            return ss;
        }
    }
}
