// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
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
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                DigitalSignatureService dss = new DigitalSignatureService();
                RemoteTemplatesSource rts = new RemoteTemplatesSource(Platforms.Uwp, ProgrammingLanguages.CSharp, new DigitalSignatureService());
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
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Fact]
        public async Task DigitalSignaturService_VerifySignatures()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(DigitalSignaturServiceTests).Assembly.CodeBase).LocalPath);
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                DigitalSignatureService dss = new DigitalSignatureService();
                RemoteTemplatesSource rts = new RemoteTemplatesSource(Platforms.Uwp, ProgrammingLanguages.CSharp, new DigitalSignatureService());
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
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
        }

        [Fact]
        public void DigitalSignaturService_VerifyLocalContent()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(DigitalSignaturServiceTests).Assembly.CodeBase).LocalPath);
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                DigitalSignatureService dss = new DigitalSignatureService();
                var packageFile = Path.GetFullPath(@".\Packaging\MsSigned\Templates.mstx");

                using (Package package = Package.Open(packageFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var isSigned = dss.IsSigned(package);
                    var isSignatureValid = dss.VerifySignatures(package);

                    Assert.True(isSigned);
                    Assert.True(isSignatureValid);
                }
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
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

            var file = Path.GetFullPath(@".\Packaging\Unsigned\JustPacked.mstx");

            using (Package package = Package.Open(file, FileMode.Open))
            {
                dss.SignPackage(package, cert);

            }

            using (Package package = Package.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var isSigned = dss.IsSigned(package);
                var isSignatureValid = dss.VerifySignatures(package);

                Assert.True(isSigned);
                Assert.True(isSignatureValid);
            }

            File.Delete(file);
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
