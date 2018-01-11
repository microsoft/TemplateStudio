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
        public void Pack_Folder()
        {
            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"C:\Temp\PackTests";
            var outFile = Path.Combine(outDir, "JustPacked.mstx");
            var extractDir = Path.Combine(outDir, "Extraction");

            TemplatePackage.Pack(inFolder, outFile, MediaTypeNames.Text.Plain);

            TemplatePackage.Extract(outFile, extractDir, false);

            int filesInExtractionFolder = new DirectoryInfo(extractDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            Directory.Delete(outDir, true);
        }

        [Fact]
        public void Pack_FolderWithDefaultNaming()
        {
            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"C:\Temp\PackTests";
            var extractDir = Path.Combine(outDir, "Extraction");

            var outFile = TemplatePackage.Pack(inFolder);

            TemplatePackage.Extract(outFile, extractDir, false);

            int filesInExtractionFolder = new DirectoryInfo(extractDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            File.Delete(outFile);
            Directory.Delete(outDir, true);
        }

        [Fact]
        public void PackAndSign_Folder()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"OutFolder\Extraction";

            string signedFile = TemplatePackage.PackAndSign(inFolder, cert);
            TemplatePackage.Extract(signedFile, outDir);

            int filesInExtractionFolder = new DirectoryInfo(outDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            File.Delete(signedFile);
            Directory.Delete(outDir, true);
        }

        [Fact]
        public void PackAndSign_FolderExtractToAbsoluteDir()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"C:\Temp\OutFolder\Extraction";

            string signedFile = TemplatePackage.PackAndSign(inFolder, cert);
            TemplatePackage.Extract(signedFile, outDir);

            int filesInExtractionFolder = new DirectoryInfo(outDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            File.Delete(signedFile);
            Directory.Delete(outDir, true);
        }

        [Fact]
        public void PackAndSign_CertNotFound()
        {
            Exception ex = Assert.Throws<SignCertNotFoundException>(() =>
            {
                TemplatePackage.PackAndSign(@"Packaging\SampleContent.txt", "SignedContent.package", "CERT_NOT_FOUND", MediaTypeNames.Text.Plain);
            });
        }

        [Fact]
        public void PackAndSign_CertFromFile_RelativeInOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"Packaging\SignedContent.package";

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void PackAndSign_CertFromFile_AbsoluteInRelativeOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Packaging\SampleContent.txt");
            var outFile = @"Packaging\SignedContent.package";

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void PackAndSign_CertFromFile_RelativeInAbsouluteOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"C:\temp\Packaging\SignedContent.package";

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void PackAndSign_CertFromFile_AbsouluteInOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Packaging\SampleContent.txt");
            var outFile = @"C:\temp\Packaging\SignedContent.package";

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void PackAndSign_WithThumbprint()
        {
            EnsureTestCertificateInStore();

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Packaging\SampleContent.txt");
            var outFile = @"C:\temp\Packaging\SignedContent.package";

            TemplatePackage.PackAndSign(inFile, outFile, "B584589A382B2AD20B54D2DD1634BB487792A970", MediaTypeNames.Text.Plain);

            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void ExtractRelativeDirs()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"Packaging\ToExtract.package";
            var extractionDir = "NewDirToExtract";

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            TemplatePackage.Extract(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, inFile)));

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }

        [Fact]
        public void ExtractAbsoluteDirs()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Packaging\SampleContent.txt");
            var outFile = @"C:\Temp\MyPackage\ToExtract.package";
            var extractionDir = @"C:\Temp\NewContent\Extracted";

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            TemplatePackage.Extract(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, @"Packaging\SampleContent.txt")));

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }

        [Fact]
        public void ExtractFileAndPacksInCurrentDir()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            File.Copy(@"Packaging\SampleContent.txt", Path.Combine(Environment.CurrentDirectory, "NewFile.txt"), true);
            var inFile = "NewFile.txt";
            var outFile = @"ToExtract.package";
            var extractionDir = Environment.CurrentDirectory;

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            TemplatePackage.Extract(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, Path.GetFileName(inFile))));

            File.Delete(outFile);
        }

        [Fact]
        public void ExtractFileCurrentDir()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"ToExtract.package";
            var extractionDir = string.Empty;

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            TemplatePackage.Extract(outFile, extractionDir);

            Assert.True(File.Exists(outFile));

            File.Delete(outFile);
        }

        [Fact]
        public async Task ExtractConcurrentReadAsync()
        {
            var inFile = @"Packaging\MsSigned\Templates.mstx";
            var outDir1 = @"C:\Temp\OutFolder\Concurrent1";
            var outDir2 = @"C:\Temp\OutFolder\Concurrent2";

            Task t1 = new Task(() => TemplatePackage.Extract(inFile, outDir1));
            Task t2 = new Task(() => TemplatePackage.Extract(inFile, outDir2));

            t1.Start();
            t2.Start();

            await Task.WhenAll(t1, t2);

            Directory.Delete(outDir1, true);
            Directory.Delete(outDir2, true);
        }

        [Fact]
        public void ExtractFileTampered()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"Packaging\ToExtract.package";
            var extractionDir = "SubDir";

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            ModifyContent(outFile, "SampleContent.txt");

            Exception ex = Assert.Throws<InvalidSignatureException>(() =>
            {
                TemplatePackage.Extract(outFile, extractionDir);
            });

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }

        [Fact]
        public void ValidateSignatureTamperedPackage()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = TemplatePackage.LoadCert(@"Packaging\TestCert.pfx", certPass);

            var inFile = @"Packaging\SampleContent.txt";
            var outFile = @"Packaging\ToExtract.package";

            TemplatePackage.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

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
        public void TestRemoteSource_Acquire()
        {
            RemoteTemplatesSource rts = new RemoteTemplatesSource();
            rts.LoadConfig();
            var package = rts.Config.Latest;

            rts.Acquire(ref package);

            string acquiredContentFolder = package.LocalPath;

            Assert.NotNull(acquiredContentFolder);

            // Ensure package is not downloaded again if already downloaded
            rts.Acquire(ref package);
            Assert.True(acquiredContentFolder == package.LocalPath);

            // Reset localPath and ensure it is acquired again
            if (Directory.Exists(Path.GetDirectoryName(package.LocalPath)))
            {
                Directory.Delete(Path.GetDirectoryName(package.LocalPath), true);
            }

            rts.Acquire(ref package);

            Assert.True(package.LocalPath != acquiredContentFolder);

            if (Directory.Exists(Path.GetDirectoryName(package.LocalPath)))
            {
                Directory.Delete(Path.GetDirectoryName(package.LocalPath), true);
            }
        }

        [Fact]
        public void TestRemoteSource_GetContent()
        {
            string drive = Path.GetPathRoot(new Uri(typeof(TemplatePackageTests).Assembly.CodeBase).LocalPath);
            string testDir = Path.Combine(drive, $@"Temp\TestRts{Process.GetCurrentProcess().Id}_{Thread.CurrentThread.ManagedThreadId}");

            try
            {
                RemoteTemplatesSource rts = new RemoteTemplatesSource();
                rts.LoadConfig();
                var package = rts.Config.Latest;

                rts.Acquire(ref package);
                var contentInfo = rts.GetContent(package, testDir);

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
