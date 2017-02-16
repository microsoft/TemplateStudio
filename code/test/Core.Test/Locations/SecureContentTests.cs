using Microsoft.Templates.Core.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Templates.Core.Test.Locations
{
    public class SecureContentTests
    {
        [Fact]
        public void SignAndPackCertNotFound()
        {
            Exception ex = Assert.Throws<SignCertNotFoundException>(() => {
                SecureContent.PackAndSign(@"Locations\SampleContent.txt", "SignedContent.package", "CERT_NOT_FOUND");
            });
        }


        [Fact]
        public void SignAndPack_CertFromFile_RelativeInOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = SecureContent.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = @"Locations\SampleContent.txt";
            var outFile = @"Locations\SignedContent.package";

            SecureContent.PackAndSign(inFile, outFile, cert);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void SignAndPack_CertFromFile_AbsoluteInRelativeOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = SecureContent.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = Path.Combine(Environment.CurrentDirectory, @"Locations\SampleContent.txt");
            var outFile = @"Locations\SignedContent.package";

            SecureContent.PackAndSign(@"Locations\SampleContent.txt", outFile, cert);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void SignAndPack_CertFromFile_RelativeInAbsouluteOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = SecureContent.LoadCert(@"Locations\TestCert.pfx", certPass);

            var inFile = @"Locations\SampleContent.txt";
            var outFile = @"C:\temp\Locations\SignedContent.package";

            SecureContent.PackAndSign(inFile, outFile, cert);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void SignAndPack_CertFromFile_AbsouluteInOutPath()
        {
            var certPass = GetTestCertPassword();
            X509Certificate2 cert = SecureContent.LoadCert(@"Locations\TestCert.pfx", certPass);

            var outFile = @"C:\temp\Locations\SignedContent.package";
            var inFile = @"Locations\SampleContent.txt";

            SecureContent.PackAndSign(inFile, outFile, cert);
            Assert.True(File.Exists(outFile));
            File.Delete(outFile);
        }

        [Fact]
        public void SignAndPackAddCertToStore()
        {
            SecureString ss = GetTestCertPassword();
            bool removeCertFromStore = false;
            if (SecureContent.LoadCert("B584589A382B2AD20B54D2DD1634BB487792A970") == null)
            {
                X509Certificate2 c = new X509Certificate2(@"Locations\TestCert.pfx", ss, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Add(c);

                removeCertFromStore = true;
            }

            X509Certificate2 cert = SecureContent.LoadCert(@"Locations\TestCert.pfx", ss);
            SecureContent.PackAndSign(@"Locations\SampleContent.txt", "SignedContent.package", "CERT_NOT_FOUND");

            if (removeCertFromStore)
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Remove(SecureContent.LoadCert("B584589A382B2AD20B54D2DD1634BB487792A970"));

                removeCertFromStore = true;
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
