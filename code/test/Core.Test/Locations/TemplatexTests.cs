using Microsoft.Templates.Core.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class TemplatexTests
    {
        [Fact]
        public void PackFolder()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"OutFolder\Extraction";

            string signedFile = Templatex.PackAndSign(inFolder, cert);
            Templatex.Extract(signedFile, outDir);

            int filesInExtractionFolder = new DirectoryInfo(outDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            File.Delete(signedFile);
            Directory.Delete(outDir, true);
        }

        [Fact]
        public void PackFolderExtractToAbsoluteDir()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            int filesInCurrentFolder = new DirectoryInfo(Environment.CurrentDirectory).GetFiles("*", SearchOption.AllDirectories).Count();
            var inFolder = Environment.CurrentDirectory;
            var outDir = @"C:\Temp\OutFolder\Extraction";

            string signedFile = Templatex.PackAndSign(inFolder, cert);
            Templatex.Extract(signedFile, outDir);

            int filesInExtractionFolder = new DirectoryInfo(outDir).GetFiles("*", SearchOption.AllDirectories).Count();
            Assert.Equal(filesInCurrentFolder, filesInExtractionFolder);

            File.Delete(signedFile);
            Directory.Delete(outDir, true);
        }

        [Fact]
        public void PackAndSignCertNotFound()
        {
            Exception ex = Assert.Throws<SignCertNotFoundException>(() => {
                Templatex.PackAndSign(@"Locations\SampleContent.txt", "SignedContent.package", "CERT_NOT_FOUND", MediaTypeNames.Text.Plain);
            });
        }


        [Fact]
        public void PackAndSign_CertFromFile_RelativeInOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = @"Locations\SampleContent.txt";
            var outFile = @"Locations\SignedContent.package";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void PackAndSign_CertFromFile_AbsoluteInRelativeOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Locations\SampleContent.txt");
            var outFile = @"Locations\SignedContent.package";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void PackAndSign_CertFromFile_RelativeInAbsouluteOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = @"Locations\SampleContent.txt";
            var outFile = @"C:\temp\Locations\SignedContent.package";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void PackAndSign_CertFromFile_AbsouluteInOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Locations\SampleContent.txt");
            var outFile = @"C:\temp\Locations\SignedContent.package";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void PackAndSignWithThumbprint()
        {
            EnsureTestCertificateInStore();

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Locations\SampleContent.txt");
            var outFile = @"C:\temp\Locations\SignedContent.package";

            Templatex.PackAndSign(inFile, outFile, "B584589A382B2AD20B54D2DD1634BB487792A970", MediaTypeNames.Text.Plain);

            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void ExtractRelativeDirs()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = @"Locations\SampleContent.txt";
            var outFile = @"Locations\ToExtract.package";
            var extractionDir = "NewDirToExtract";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            Templatex.Extract(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, inFile)));

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }



        [Fact]
        public void ExtractAbsoluteDirs()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Locations\SampleContent.txt");
            var outFile = @"C:\Temp\MyPackage\ToExtract.package";
            var extractionDir = @"C:\Temp\NewContent\Extracted";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            Templatex.Extract(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, @"Locations\SampleContent.txt")));

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }

        [Fact]
        public void ExtractFileAndPacksInCurrentDir()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            File.Copy(@"Locations\SampleContent.txt", Path.Combine(Environment.CurrentDirectory, "NewFile.txt"),true);
            var inFile = "NewFile.txt";
            var outFile = @"ToExtract.package";
            var extractionDir = Environment.CurrentDirectory;

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            Templatex.Extract(outFile, extractionDir);

            Assert.True(Directory.Exists(extractionDir));
            Assert.True(File.Exists(Path.Combine(extractionDir, Path.GetFileName(inFile))));

            File.Delete(outFile);
        }

        [Fact]
        public void ExtractFileCurrentDir()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = @"Locations\SampleContent.txt";
            var outFile = @"ToExtract.package";
            var extractionDir = "";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            Templatex.Extract(outFile, extractionDir);

            Assert.True(File.Exists(Path.Combine(extractionDir, Path.GetFileName(inFile))));

            File.Delete(outFile);
        }

        [Fact]
        public void ExtractFileTampered()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = @"Locations\SampleContent.txt";
            var outFile = @"Locations\ToExtract.package";
            var extractionDir = "SubDir";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            ModifyContent(outFile, "SampleContent.txt");

            Exception ex = Assert.Throws<InvalidSignatureException>(() => {
                Templatex.Extract(outFile, extractionDir);
            });

            File.Delete(outFile);
            Directory.Delete(extractionDir, true);
        }

        [Fact]
        public void ValidateSignature()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = Templatex.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = @"Locations\SampleContent.txt";
            var outFile = @"Locations\ToExtract.package";

            Templatex.PackAndSign(inFile, outFile, cert, MediaTypeNames.Text.Plain);

            ModifyContent(outFile, "SampleContent.txt");

            Assert.False(Templatex.ValidateSignatures(outFile));

            File.Delete(outFile);
        }

        [Fact]
        public void UseRemote()
        {
            RemoteTemplatesLocation remote = new RemoteTemplatesLocation();
            var copyResult = remote.Copy(@"C:\Temp\WorkingFolder");
            Assert.Equal(copyResult.Status, LocationCopyStatus.Finished);

            remote.GetVersion(@"C:\Temp\WorkingFolder");
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
            if (Templatex.LoadCert("B584589A382B2AD20B54D2DD1634BB487792A970") == null)
            {
                X509Certificate2 c = new X509Certificate2(@"Locations\TestCert.pfx", ss, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

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
