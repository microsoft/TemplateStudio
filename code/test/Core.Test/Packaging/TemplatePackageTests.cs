// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;

using Xunit;

namespace Microsoft.Templates.Core.Test.Locations
{
    [Trait("ExecutionSet", "Minimum")]

    public class TemplatePackageTests
    {
        [Fact]
        public async Task Pack_FolderAsync()
        {
            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"C:\Temp\PackTests";
            var outFile = Path.Combine(outDir, "JustPacked.mstx");
            var extractDir = Path.Combine(outDir, "Extraction");

            await TemplatePackage.PackAsync(inFolder, outFile, MediaTypeNames.Text.Plain);

            await TemplatePackage.ExtractAsync(outFile, extractDir, false);

            int filesInExtractionFolder = new DirectoryInfo(extractDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            Directory.Delete(outDir, true);
        }

        [Fact]
        public async Task Pack_FolderWithDefaultNamingAsync()
        {
            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"C:\Temp\PackTests";
            var extractDir = Path.Combine(outDir, "Extraction");

            var outFile = await TemplatePackage.PackAsync(inFolder);

            await TemplatePackage.ExtractAsync(outFile, extractDir, false);

            int filesInExtractionFolder = new DirectoryInfo(extractDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            File.Delete(outFile);
            Directory.Delete(outDir, true);
        }

        [Fact]
        public async Task PackAndSign_FolderAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"OutFolder\Extraction";

            string signedFile = await TemplatePackage.PackAndSignAsync(inFolder, cert);
            await TemplatePackage.ExtractAsync(signedFile, outDir);

            int filesInExtractionFolder = new DirectoryInfo(outDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            File.Delete(signedFile);
            Directory.Delete(outDir, true);
        }

        [Fact]
        public async Task PackAndSign_FolderExtractToAbsoluteDirAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"C:\Temp\OutFolder\Extraction";

            string signedFile = await TemplatePackage.PackAndSignAsync(inFolder, cert);
            await TemplatePackage.ExtractAsync(signedFile, outDir);

            int filesInExtractionFolder = new DirectoryInfo(outDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            File.Delete(signedFile);
            Directory.Delete(outDir, true);
        }

        [Fact]
        public async Task PackAndSign_CertNotFoundAsync()
        {
            SignCertNotFoundException ex = await Assert.ThrowsAsync<SignCertNotFoundException>(async () =>
            {
                await TemplatePackage.PackAndSignAsync(@"Packaging\SampleContent.txt", "SignedContent.package", "CERT_NOT_FOUND", MediaTypeNames.Text.Plain);
            });
        }

        [Fact]
        public async Task PackAndSign_CertFromFile_RelativeInOutPathAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"Packaging\SignedContent.package";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public async Task PackAndSign_CertFromFile_AbsoluteInRelativeOutPathAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Packaging\SampleContent.txt");
            var outFile = @"Packaging\SignedContent.package";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public async Task PackAndSign_CertFromFile_RelativeInAbsouluteOutPathAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"C:\temp\Packaging\SignedContent.package";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public async Task PackAndSign_CertFromFile_AbsouluteInOutPathAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Packaging\SampleContent.txt");
            var outFile = @"C:\temp\Packaging\SignedContent.package";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public async Task PackAndSign_WithThumbprintAsync()
        {
            EnsureTestCertificateInStore();

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Packaging\SampleContent.txt");
            var outFile = @"C:\temp\Packaging\SignedContent.package";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, "B584589A382B2AD20B54D2DD1634BB487792A970", MediaTypeNames.Text.Plain);

            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public async Task ExtractRelativeDirsAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"Packaging\ToExtract.package";
            var extractionDir = "NewDirToExtract";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            await TemplatePackage.ExtractAsync(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, inFile)));

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }

        [Fact]
        public async Task ExtractAbsoluteDirsAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Packaging\SampleContent.txt");
            var outFile = @"C:\Temp\MyPackage\ToExtract.package";
            var extractionDir = @"C:\Temp\NewContent\Extracted";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            await TemplatePackage.ExtractAsync(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, @"Packaging\SampleContent.txt")));

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }

        [Fact]
        public async Task ExtractFileAndPacksInCurrentDirAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            File.Copy(@"Packaging\SampleContent.txt", Path.Combine(Environment.CurrentDirectory, "NewFile.txt"), true);
            var inFile = "NewFile.txt";
            var outFile = @"ToExtract.package";
            var extractionDir = Environment.CurrentDirectory;

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            await TemplatePackage.ExtractAsync(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, Path.GetFileName(inFile))));

            File.Delete(outFile);
        }

        [Fact]
        public async Task ExtractFileCurrentDirAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"ToExtract.package";
            var extractionDir = string.Empty;

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            await TemplatePackage.ExtractAsync(outFile, extractionDir);

            Assert.True(File.Exists(outFile));

            File.Delete(outFile);
        }

        [Fact]
        public async Task ExtractConcurrentReadAsync()
        {
            var inFile = @"Packaging\MsSigned\Templates.mstx";
            var outDir1 = @"C:\Temp\OutFolder\Concurrent1";
            var outDir2 = @"C:\Temp\OutFolder\Concurrent2";

            Task t1 = Task.Run(async () => await TemplatePackage.ExtractAsync(inFile, outDir1));
            Task t2 = Task.Run(async () => await TemplatePackage.ExtractAsync(inFile, outDir2));

            await Task.WhenAll(t1, t2);

            Directory.Delete(outDir1, true);
            Directory.Delete(outDir2, true);
        }

        [Fact]
        public async Task ExtractFileTamperedAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"Packaging\ToExtract.package";
            var extractionDir = "SubDir";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            ModifyContent(outFile, "SampleContent.txt");

            InvalidSignatureException ex = await Assert.ThrowsAsync<InvalidSignatureException>(async () =>
            {
                await TemplatePackage.ExtractAsync(outFile, extractionDir);
            });

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }

        [Fact]
        public async Task ValidateSignatureTamperedPackageAsync()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"Packaging\ToExtract.package";

            await TemplatePackage.PackAndSignAsync(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            ModifyContent(outFile, "SampleContent.txt");

            Assert.False(TemplatePackage.ValidateSignatures(outFile));

            File.Delete(outFile);
        }

        [Fact]
        public void ValidateSignatureFromMsSigned()
        {
            var msSignedFile = @"Packaging\MsSigned\Templates.mstx";
            Assert.True(TemplatePackage.ValidateSignatures(msSignedFile));
        }

        // TODO: Refactor this methods to other class
        [Fact]
        public async Task TestRemoteSource_AcquireAsync()
        {
            RemoteTemplatesSource rts = new RemoteTemplatesSource();
            CancellationTokenSource cts = new CancellationTokenSource();

            await rts.LoadConfigAsync(cts.Token);
            var package = rts.Config.Latest;

            await rts.AcquireAsync(package, cts.Token);

            string acquiredContentFolder = package.LocalPath;

            Assert.NotNull(acquiredContentFolder);

            // Ensure package is not downloaded again if already downloaded
            await rts.AcquireAsync(package, cts.Token);
            Assert.True(acquiredContentFolder == package.LocalPath);

            // Reset localPath and ensure it is acquired again
            if (Directory.Exists(Path.GetDirectoryName(package.LocalPath)))
            {
                Directory.Delete(Path.GetDirectoryName(package.LocalPath), true);
            }

            await rts.AcquireAsync(package, cts.Token);

            Assert.True(package.LocalPath != acquiredContentFolder);

            if (Directory.Exists(Path.GetDirectoryName(package.LocalPath)))
            {
                Directory.Delete(Path.GetDirectoryName(package.LocalPath), true);
            }
        }

        [Fact]
        public async Task TestRemoteSource_GetContentAsync()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(TemplatePackageTests).Assembly.CodeBase).LocalPath);
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                RemoteTemplatesSource rts = new RemoteTemplatesSource();
                CancellationTokenSource cts = new CancellationTokenSource();
                await rts.LoadConfigAsync(cts.Token);
                var package = rts.Config.Latest;

                await rts.AcquireAsync(package, cts.Token);
                var contentInfo = await rts.GetContentAsync(package, testDir);

                Assert.True(Directory.Exists(contentInfo.Path));
            }
            finally
            {
                if (Directory.Exists(testDir))
                {
                    Directory.Delete(testDir, true);
                }
            }
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

        private static void EnsureTestCertificateInStore()
        {
            SecureString ss = GetTestCertPassword();
            if (TemplatePackage.LoadCert("B584589A382B2AD20B54D2DD1634BB487792A970") == null)
            {
                X509Certificate2 c = new X509Certificate2(@"Packaging\TestCert.pfx", ss, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Add(c);
                store.Close();
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
