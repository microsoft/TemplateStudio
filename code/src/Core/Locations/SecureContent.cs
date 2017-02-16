using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Mime;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Locations
{
    public class SecureContent
    {
        const string TemplatesContentRelationshipType = "http://schemas.microsoft.com/opc/2006/06/templates/content";

        public static void PackAndSign(string sourceFile, string signedPackageFilename, string certThumbprint, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            X509Certificate cert = LoadCert(certThumbprint);
            if(cert == null)
            {
                throw new SignCertNotFoundException($"The certificate with thumbprint {certThumbprint} can't be found in the stores CurrentUser or LocalMachine.");
            }
            PackAndSign(sourceFile, signedPackageFilename, cert, mimeMediaType);
        }
        public static void PackAndSign(string sourceFile, string signedPackageFilename, X509Certificate signingCert, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            Uri uriFile = new Uri(sourceFile, UriKind.Relative);
            Uri partUriFile = PackUriHelper.CreatePartUri(uriFile);

            string targetFileDir = Path.GetDirectoryName(signedPackageFilename);
            if (!Directory.Exists(targetFileDir)) Directory.CreateDirectory(targetFileDir);

            using (Package package = Package.Open(signedPackageFilename, FileMode.Create))
            {
                 PackagePart packagePart = package.CreatePart(partUriFile, mimeMediaType);

                // Add content to the File part
                using (FileStream fileStream = new FileStream(uriFile.ToString(), FileMode.Open, FileAccess.Read))
                {
                    CopyStream(fileStream, packagePart.GetStream());
                }

                // Add a Package Relationship to the Document Part.
                package.CreateRelationship(packagePart.Uri, TargetMode.Internal, TemplatesContentRelationshipType);

                
                package.Flush();

                SignAllParts(package, signingCert);
            }

        }

        public static void Extract(string signedPackageFilename, string targetDirectory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(targetDirectory);
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
            directoryInfo = Directory.CreateDirectory(targetDirectory);

            // Move the current Package to the Target directory.
            string targetFilename = targetDirectory + @"\" + signedPackageFilename;
            File.Copy(signedPackageFilename, targetFilename);

            // Open the Package copy in the target directory.
            using (Package package = Package.Open(targetFilename, FileMode.Open, FileAccess.Read))
            {
                if (ValidateSignatures(package))
                {
                    PackagePart packagePartDocument = null;
                   
                    foreach (PackageRelationship relationship in package.GetRelationshipsByType(TemplatesContentRelationshipType))
                    {
                        // Open the Document part, write the contents to a file.
                        packagePartDocument = package.GetPart(relationship.TargetUri);
                        ExtractPart(packagePartDocument, targetFilename);
                    }
                }
                else
                {
                    string msg = $"Digital signatures in '{signedPackageFilename}\n' failed validation.  Unable to continue.";
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
            }
            return certFound;
        }
        private static void ExtractPart(PackagePart packagePart, string packageFilename)
        {
            string stringPart = packagePart.Uri.ToString().TrimStart('/');
            Uri partUri = new Uri(stringPart, UriKind.Relative);

            Uri uriFullPartPath = new Uri(new Uri(packageFilename, UriKind.Absolute), partUri);

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
            if (package == null)
                throw new ArgumentNullException("SignAllParts(package)");

            PackageDigitalSignatureManager dsm = new PackageDigitalSignatureManager(package)
            {
                CertificateOption = CertificateEmbeddingOption.InCertificatePart,
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

    }

}
