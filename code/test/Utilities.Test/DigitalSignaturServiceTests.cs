// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;
using Microsoft.Templates.Utilities.Services;
using Xunit;

namespace Microsoft.Templates.Utilities
{
    [Trait("ExecutionSet", "Minimum")]
    public class DigitalSignaturServiceTests
    {

        [Fact]
        public async Task DigitalSignaturService_IsSigned()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(DigitalSignaturServiceTests).Assembly.CodeBase).LocalPath);

            DigitalSignatureService dss = new DigitalSignatureService();
            RemoteTemplatesSource rts = new RemoteTemplatesSource(Platforms.Uwp, ProgrammingLanguages.CSharp, null, new DigitalSignatureService());
            CancellationTokenSource cts = new CancellationTokenSource();
            await rts.LoadConfigAsync(cts.Token);
            var templatesPackage = rts.Config.Latest;

            await rts.AcquireAsync(templatesPackage, cts.Token);
            using (Package package = Package.Open(templatesPackage.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var isSigned = dss.IsSigned(package);

                Assert.True(isSigned);
            }
        }

        [Fact]
        public async Task DigitalSignaturService_VerifySignatures()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(DigitalSignaturServiceTests).Assembly.CodeBase).LocalPath);

            DigitalSignatureService dss = new DigitalSignatureService();
            RemoteTemplatesSource rts = new RemoteTemplatesSource(Platforms.Uwp, ProgrammingLanguages.CSharp, null, new DigitalSignatureService());
            CancellationTokenSource cts = new CancellationTokenSource();
            await rts.LoadConfigAsync(cts.Token);
            var templatesPackage = rts.Config.Latest;

            await rts.AcquireAsync(templatesPackage, cts.Token);
            using (Package package = Package.Open(templatesPackage.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var signatureValid = dss.VerifySignatures(package);

                Assert.True(signatureValid);
            }
        }

        [Fact]
        public void DigitalSignaturService_VerifyLocalContent()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(DigitalSignaturServiceTests).Assembly.CodeBase).LocalPath);

            DigitalSignatureService dss = new DigitalSignatureService();
            var packageFile = Path.GetFullPath(@".\Packaging\MsSigned\Templates.mstx");

            using (Package package = Package.Open(packageFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var isSigned = dss.IsSigned(package);
                var isSignatureValid = dss.VerifySignatures(package);

                Assert.True(isSigned);
                Assert.True(isSignatureValid);

                foreach (var cert in dss.GetX509Certificates(package))
                {
                    var statusFlag = dss.VerifyCertificate(cert);
                    Assert.NotEqual(X509ChainStatusFlags.Revoked, statusFlag);
                }
            }  
        }

        [Fact]
        public void SignPackage()
        {
            var dss = new DigitalSignatureService();
            var templatePackage = new TemplatePackage(dss);
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = templatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var file = Path.GetFullPath(@".\Packaging\Unsigned\UnsignedPackage.mstx");
            var file2 = Path.GetFullPath(@".\Packaging\ToSign.mstx");
            File.Copy(file, file2);

            using (Package package = Package.Open(file2, FileMode.Open))
            {
                dss.SignPackage(package, cert);
            }

            using (Package package = Package.Open(file2, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var isSigned = dss.IsSigned(package);
                var isSignatureValid = dss.VerifySignatures(package);

                Assert.True(isSigned);
                Assert.True(isSignatureValid);
            }

            File.Delete(file2);
        }

        [Fact]
        public void ExtractPackage_Tampered()
        {
            var dss = new DigitalSignatureService();
            var templatePackage = new TemplatePackage(dss);
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = templatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var file = Path.GetFullPath(@".\Packaging\Unsigned\UnsignedPackage.mstx");
            var file2 = Path.GetFullPath(@".\Packaging\ToSign.mstx");
            File.Copy(file, file2);


            using (Package package = Package.Open(file2, FileMode.Open))
            {
                dss.SignPackage(package, cert);

            }

            ModifyContent(file2, "SampleContent.txt");

            using (Package package = Package.Open(file2, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var isSigned = dss.IsSigned(package);
                var isSignatureValid = dss.VerifySignatures(package);

                Assert.True(isSigned);
                Assert.False(isSignatureValid);
            }

            File.Delete(file2);
        }

        private void ModifyContent(string signedPack, string contentFile)
        {
            using (ZipArchive zip = ZipFile.Open(signedPack, ZipArchiveMode.Update))
            {
                var entry = zip.Entries.Where(e => e.Name == contentFile).FirstOrDefault();
                if (entry != null)
                {
                    using (StreamWriter sw = new StreamWriter(entry.Open()))
                    {
                        sw.BaseStream.Position = sw.BaseStream.Length - 1;
                        sw.WriteLine("Tampered");
                    }
                }
            }
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
