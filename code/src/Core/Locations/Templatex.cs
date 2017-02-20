using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public class Templatex
    {
        public const string DefaultExtension = ".mstx";
        const string TemplatesContentRelationshipType = "http://schemas.microsoft.com/opc/2006/06/templates/securecontent";

        public static string PackAndSign(string source, string certThumbprint, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string signedPack = source + DefaultExtension;
            PackAndSign(source, signedPack, certThumbprint, mimeMediaType);
            return signedPack;
        }
        public static string PackAndSign(string source, X509Certificate signingCert, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string signedPack = source + DefaultExtension;
            PackAndSign(source, signedPack, signingCert, mimeMediaType);
            return signedPack;
        } 
        public static void PackAndSign(string source, string signedFilePack, string certThumbprint, string mimeMediaType)
        {
            X509Certificate cert = LoadCert(certThumbprint);
            if(cert == null)
            {
                throw new SignCertNotFoundException($"The certificate with thumbprint {certThumbprint} can't be found in CurrentUser/My or LocalMachine/My.");
            }
            PackAndSign(source, signedFilePack, cert, mimeMediaType);
        }
        public static void PackAndSign(string source, string signedPack, X509Certificate signingCert, string mimeMediaType)
        {
            if (String.IsNullOrWhiteSpace(source)) throw new ArgumentException("source");
            if (String.IsNullOrWhiteSpace(signedPack)) throw new ArgumentException("signedPack");
            if (signingCert == null) throw new ArgumentException("signingCert");

            FileInfo[] files = GetSourceFiles(source);

            Uri rootUri = GetRootUri(source);

            EnsureDirectory(Path.GetDirectoryName(signedPack)); 

            using (Package package = Package.Open(signedPack, FileMode.Create))
            {
                foreach (var file in files)
                {
                    Uri partUriFile = GetPartUriFile(rootUri, file);

                    PackagePart packagePart = package.CreatePart(partUriFile, mimeMediaType, CompressionOption.Maximum);

                    AddContentToPackagePart(file, packagePart);

                    package.CreateRelationship(packagePart.Uri, TargetMode.Internal, TemplatesContentRelationshipType);
                }

                package.Flush();

                SignAllParts(package, signingCert);
            }
        }


        public static void Extract(string signedFilePack, string targetDirectory)
        {
            string currentDir = Environment.CurrentDirectory;
            string inFilePack = Path.IsPathRooted(signedFilePack) ? signedFilePack : Path.Combine(currentDir, signedFilePack);
            string outDir = Path.IsPathRooted(targetDirectory) ? targetDirectory : Path.Combine(currentDir, targetDirectory);

            EnsureDirectory(outDir);

            using (Package package = Package.Open(inFilePack, FileMode.Open, FileAccess.Read))
            {
                if (ValidateSignatures(package))
                {
                    PackagePart packagePartDocument = null;

                    foreach (PackageRelationship relationship in package.GetRelationshipsByType(TemplatesContentRelationshipType))
                    {
                        // Open the Document part, write the contents to a file.
                        packagePartDocument = package.GetPart(relationship.TargetUri);
                        ExtractPart(packagePartDocument, outDir);
                    }
                }
                else
                {
                    string msg = $"Digital signatures in '{signedFilePack}\n' failed validation.  Unable to continue.";
                    throw new InvalidSignatureException(msg);
                }
            }
        }

        public static bool ValidateSignatures(string signedPackageFilename)
        {
            using (Package package = Package.Open(signedPackageFilename, FileMode.Open, FileAccess.Read))
            {
                return ValidateSignatures(package);
            }
        }
        public static X509Certificate2 LoadCert(string filePath, SecureString password)
        {
            X509Certificate2 cert = new X509Certificate2();
            cert.Import(filePath, password, X509KeyStorageFlags.DefaultKeySet);
            return cert;
        }

        public static X509Certificate2 LoadCert(string thumbprint)
        {
            X509Certificate2 certFound = FindCertificate(thumbprint, StoreLocation.CurrentUser);
            if (certFound == null)
            {
                certFound = FindCertificate(thumbprint, StoreLocation.LocalMachine);
            }
            if (certFound == null)
            {
                AppHealth.Current.Warning.TrackAsync($"No certificate found matching the thumbrint {thumbprint}. Searched on CurrentUser/My and LocalMachine/My stores.").FireAndForget();
            }
            return certFound;
        }

        private static X509Certificate2 FindCertificate(string thumbprint, StoreLocation location)
        {
            X509Certificate2 certFound = null;
            using (X509Store store = new X509Store(StoreName.My, location))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                AppHealth.Current.Info.TrackAsync($"Found {certs.Count} certificates matching the thumbprint {thumbprint} in the store {location.ToString()}").FireAndForget();
                if (certs.Count >= 1)
                {
                    certFound = certs[0];
                    if (certs.Count > 1)
                    {
                        AppHealth.Current.Warning.TrackAsync($"More than one certificate found matching the thumbrint. Returning the first one.").FireAndForget();
                    }

                    if (!certFound.HasPrivateKey)
                    {
                        AppHealth.Current.Info.TrackAsync($"The certificate found does not have private key.").FireAndForget();
                    }
                }
                store.Close();
            }
            return certFound;
        }
        private static void ExtractPart(PackagePart packagePart, string targetDirectory)
        {
            string stringPart = packagePart.Uri.ToString().TrimStart('/');
            
            Uri partUri = new Uri(stringPart, UriKind.Relative);

            var dir = targetDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()) ? targetDirectory : targetDirectory + Path.DirectorySeparatorChar;
            Uri uriFullPartPath = new Uri(new Uri(dir, UriKind.Absolute), partUri);

            Directory.CreateDirectory(Path.GetDirectoryName(uriFullPartPath.LocalPath));

            using (FileStream fileStream = new FileStream(uriFullPartPath.LocalPath, FileMode.Create))
            {
                CopyStream(packagePart.GetStream(), fileStream);
            }
        }

        private static bool ValidateSignatures(Package package)
        {
            if (package == null) throw new ArgumentNullException("ValidateSignatures(package)");

            PackageDigitalSignatureManager dsm = new PackageDigitalSignatureManager(package);
            bool result = dsm.IsSigned;
            if (result)
            {
                result = result && ValidateSignatureCertificates(dsm);
                if (result)
                {
                    VerifyResult verifyResult = dsm.VerifySignatures(false);
                    result = result && verifyResult == VerifyResult.Success;
                }
            }
            return result;
        }

        private static bool ValidateSignatureCertificates(PackageDigitalSignatureManager dsm)
        {
            bool certificatesOk = true;
            foreach (var signature in dsm.Signatures)
            {
                var status = PackageDigitalSignatureManager.VerifyCertificate(signature.Signer);
                certificatesOk = certificatesOk && (status == X509ChainStatusFlags.NoError);
                AppHealth.Current.Verbose.TrackAsync($"Certiticate validation finished for certificate with subject '{signature.Signer.Subject}'. Status: {status.ToString()}").FireAndForget();
            }
            AppHealth.Current.Verbose.TrackAsync($"Package certiticates valid: {certificatesOk}").FireAndForget();

            return certificatesOk;
        }

        private static void SignAllParts(Package package, X509Certificate cert)
        {
            if (package == null) throw new ArgumentNullException("SignAllParts(package)");
            if (cert == null) throw new ArgumentNullException("SignAllParts(cert)");

            PackageDigitalSignatureManager dsm = new PackageDigitalSignatureManager(package)
            {
                CertificateOption = CertificateEmbeddingOption.InSignaturePart,
                HashAlgorithm = SignedXml.XmlDsigSHA512Url
            };

            List<Uri> toSign = new List<Uri>();
            foreach (PackagePart packagePart in package.GetParts())
            {
                toSign.Add(packagePart.Uri);
            }

            toSign.Add(PackUriHelper.GetRelationshipPartUri(dsm.SignatureOrigin));

            toSign.Add(dsm.SignatureOrigin);

            toSign.Add(PackUriHelper.GetRelationshipPartUri(new Uri("/", UriKind.RelativeOrAbsolute)));

            try
            {
                  dsm.Sign(toSign, cert);
            }
            catch (CryptographicException ex)
            {
                AppHealth.Current.Error.TrackAsync("Error sigingn package.", ex).FireAndForget();
                throw;
            }
        }
        private static void CopyStream(Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }
        private static void EnsureDirectory(string dir)
        {
            if (!String.IsNullOrEmpty(dir) && dir.ToLower() != Environment.CurrentDirectory.ToLower())
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        private static void AddContentToPackagePart(FileInfo file, PackagePart packagePart)
        {
            using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                CopyStream(fileStream, packagePart.GetStream());
            }
        }

        private static Uri GetPartUriFile(Uri rootUri, FileInfo file)
        {
            Uri uriFile = rootUri.MakeRelativeUri(new Uri(file.FullName, UriKind.Absolute));
            Uri partUriFile = PackUriHelper.CreatePartUri(uriFile);
            return partUriFile;
        }

        private static Uri GetRootUri(string source)
        {
            string uriString;
            if (!Path.IsPathRooted(source))
            {
                uriString = Path.GetDirectoryName(Path.GetFullPath(source)).Replace(source, "");
                if (!File.Exists(source))
                {
                    uriString = uriString + Path.DirectorySeparatorChar;
                }
            }
            else
            {
                uriString = Path.GetDirectoryName(source);
                if (!File.Exists(source))
                {
                    uriString = uriString + Path.DirectorySeparatorChar;
                }
            }
            Uri rootUri = new Uri(uriString, UriKind.Absolute);
            return rootUri;
        }

        private static FileInfo[] GetSourceFiles(string source)
        {
            FileInfo[] files = null;
            if (!File.Exists(source))
            {
                if (Directory.Exists(Path.GetFullPath(source)))
                {
                    DirectoryInfo di = new DirectoryInfo(source);
                    files = di.GetFiles("*", SearchOption.AllDirectories);
                }
            }
            else
            {
                files = new FileInfo[] { new FileInfo(source) };
            }
            if (files == null || files.Count() == 0)
            {
                throw new FileNotFoundException($"The specified source '{source}' is invalid. Or the file does not exists or the folder is empty.");
            }

            return files;
        }
    }

}
