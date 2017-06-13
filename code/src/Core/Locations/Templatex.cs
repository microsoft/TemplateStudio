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
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Mime;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.Locations
{
    public class Templatex
    {
        const int bufSize = 0x1000;
        public const string DefaultExtension = ".mstx";
        const string TemplatesContentRelationshipType = "http://schemas.microsoft.com/opc/2006/06/templates/securecontent";

        public static string PackAndSign(string source, string certThumbprint, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string outFile = source + DefaultExtension;

            PackAndSign(source, outFile, certThumbprint, mimeMediaType);

            return outFile;
        }

        public static string PackAndSign(string source, X509Certificate signingCert, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string outFile = source + DefaultExtension;

            Pack(source, outFile, mimeMediaType);

            Sign(outFile, signingCert);

            return outFile;
        }

        public static void PackAndSign(string source, string outFile, string certThumbprint, string mimeMediaType)
        {
            X509Certificate cert = LoadCert(certThumbprint);

            if (cert == null)
            {
                throw new SignCertNotFoundException($"The certificate with thumbprint {certThumbprint} can't be found in CurrentUser/My or LocalMachine/My.");
            }

            Pack(source, outFile, mimeMediaType);

            Sign(outFile, cert);
        }

        public static void PackAndSign(string source, string outFile, X509Certificate cert, string mimeMediaType)
        {
            Pack(source, outFile, mimeMediaType);

            Sign(outFile, cert);
        }

        public static string Pack(string source, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string outFile = source + DefaultExtension;

            Pack(source, outFile, mimeMediaType);

            return outFile;
        }
        public static void Pack(string source, string outFile, string mimeMediaType)
        {
            if (string.IsNullOrWhiteSpace(source))
                throw new ArgumentException("source");

            if (string.IsNullOrWhiteSpace(outFile))
                throw new ArgumentException("outFile");

            FileInfo[] files = GetSourceFiles(source);

            Uri rootUri = GetRootUri(source);

            EnsureDirectory(Path.GetDirectoryName(outFile));

            using (Package package = Package.Open(outFile, FileMode.Create))
            {
                foreach (var file in files)
                {
                    Uri partUriFile = GetPartUriFile(rootUri, file);

                    PackagePart packagePart = package.CreatePart(partUriFile, mimeMediaType, CompressionOption.Maximum);

                    AddContentToPackagePart(file, packagePart);

                    package.CreateRelationship(packagePart.Uri, TargetMode.Internal, TemplatesContentRelationshipType);
                }

                package.Flush();
            }
        }

        public static void Extract(string signedFilePack, string targetDirectory, bool verifySignatures = true)
        {
            string currentDir = Environment.CurrentDirectory;
            string inFilePack = Path.IsPathRooted(signedFilePack) ? signedFilePack : Path.Combine(currentDir, signedFilePack);
            string outDir = Path.IsPathRooted(targetDirectory) ? targetDirectory : Path.Combine(currentDir, targetDirectory);

            EnsureDirectory(outDir);

            using (Package package = Package.Open(inFilePack, FileMode.Open, FileAccess.Read))
            {
                bool isSignatureValid = false;
                if (verifySignatures)
                {
                    isSignatureValid = ValidateSignatures(package);
                }

                if (isSignatureValid || !verifySignatures)
                {
                    ExtractContent(outDir, package);
                }

                if (!isSignatureValid && verifySignatures)
                {
                    string msg = $"Invalid digital signatures in '{signedFilePack}'. The content has been tampered or the certificate is not present, not valid or not allowed.  Unable to continue.";
                    throw new InvalidSignatureException(msg);
                }
            }
        }

        private static void Sign(string file, X509Certificate signingCert)
        {
            if (signingCert == null)
                throw new ArgumentException("signingCert");

            using (Package package = Package.Open(file, FileMode.Open))
            {
                SignAllParts(package, signingCert);
            }
        }

        private static void ExtractContent(string outDir, Package package)
        {
            PackagePart packagePartDocument = null;

            foreach (PackageRelationship relationship in package.GetRelationshipsByType(TemplatesContentRelationshipType))
            {
                // Open the Document part, write the contents to a file.
                packagePartDocument = package.GetPart(relationship.TargetUri);
                ExtractPart(packagePartDocument, outDir);
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
            var cert = new X509Certificate2();
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

            using (var store = new X509Store(StoreName.My, location))
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
        private static void ExtractPart(PackagePart packagePart, string targetDirectory)
        {
            string stringPart = packagePart.Uri.ToString().TrimStart('/');
            var partUri = new Uri(stringPart, UriKind.Relative);
            string dir = targetDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()) ? targetDirectory : targetDirectory + Path.DirectorySeparatorChar;
            var uriFullPartPath = new Uri(new Uri(dir, UriKind.Absolute), partUri);

            Directory.CreateDirectory(Path.GetDirectoryName(uriFullPartPath.LocalPath));

            using (var fileStream = new FileStream(uriFullPartPath.LocalPath, FileMode.Create))
            {
                CopyStream(packagePart.GetStream(), fileStream);
            }
        }

        private static bool ValidateSignatures(Package package)
        {
            if (package == null)
                throw new ArgumentNullException("ValidateSignatures(package)");

            var dsm = new PackageDigitalSignatureManager(package);
            bool result = dsm.IsSigned;

            if (result)
            {
                result = result && ValidatePackageCertificates(dsm);

                if (result)
                {
                    VerifyResult verifyResult = dsm.VerifySignatures(false);
                    result = result && verifyResult == VerifyResult.Success;
                }
            }

            return result;
        }

        private static bool ValidatePackageCertificates(PackageDigitalSignatureManager dsm)
        {
            Dictionary<string, X509Certificate> certs = GetPackageCertificates(dsm);
            bool certificatesOk = certs.Count > 0;

            foreach (X509Certificate cert in certs.Values)
            {
                if (CertificateChainVaidationRequired())
                {
                    certificatesOk = certificatesOk && VerifyCertificate(cert);
                }
                else
                {
                    certificatesOk = certificatesOk && VerifyAllowedPublicKey(cert);
                }

                if (!certificatesOk)
                {
                    AppHealth.Current.Warning.TrackAsync("Package signature certificate validation not passed.").FireAndForget();
                    break;
                }
            }

            return certificatesOk;
        }

        private static bool CertificateChainVaidationRequired()
        {
            return !Configuration.Current.AllowedPublicKeysPins.Where(pk => !string.IsNullOrWhiteSpace(pk)).Any();
        }

        private static Dictionary<string, X509Certificate> GetPackageCertificates(PackageDigitalSignatureManager dsm)
        {
            var certs = new Dictionary<string, X509Certificate>();

            foreach (var signature in dsm.Signatures)
            {
                if (!certs.Keys.Contains(signature.Signer.GetSerialNumberString()))
                {
                    certs.Add(signature.Signer.GetSerialNumberString(), signature.Signer);
                }
            }

            return certs;
        }

        private static bool VerifyCertificate(X509Certificate cert)
        {
            var status = PackageDigitalSignatureManager.VerifyCertificate(cert);
            AppHealth.Current.Verbose.TrackAsync($"Certificate '{cert.Subject}' verification finished with status '{status.ToString()}'").FireAndForget();

            return (status == X509ChainStatusFlags.NoError);
        }

        private static bool VerifyAllowedPublicKey(X509Certificate cert)
        {
            var pubKeyCert = cert.GetPublicKeyString();
            var pubKeyPin = pubKeyCert.ObfuscateSHA();

            AppHealth.Current.Verbose.TrackAsync($"Package certificate {cert.Subject}").FireAndForget();

            return Configuration.Current.AllowedPublicKeysPins.Where(allowedPin => allowedPin.Equals(pubKeyPin)).Any();
        }

        private static void SignAllParts(Package package, X509Certificate cert)
        {
            if (package == null)
                throw new ArgumentNullException("SignAllParts(package)");

            if (cert == null)
                throw new ArgumentNullException("SignAllParts(cert)");

            var dsm = new PackageDigitalSignatureManager(package)
            {
                CertificateOption = CertificateEmbeddingOption.InSignaturePart,
                HashAlgorithm = SignedXml.XmlDsigSHA512Url
            };

            var toSign = new List<Uri>();

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
            var buf = new byte[bufSize];
            int bytesRead = 0;

            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
            {
                target.Write(buf, 0, bytesRead);
            }
        }
        private static void EnsureDirectory(string dir)
        {
            if (!string.IsNullOrEmpty(dir) && dir.ToLower() != Environment.CurrentDirectory.ToLower())
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        private static void AddContentToPackagePart(FileInfo file, PackagePart packagePart)
        {
            using (var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                CopyStream(fileStream, packagePart.GetStream());
            }
        }

        private static Uri GetPartUriFile(Uri rootUri, FileInfo file)
        {
            var uriFile = rootUri.MakeRelativeUri(new Uri(file.FullName, UriKind.Absolute));
            var partUriFile = PackUriHelper.CreatePartUri(uriFile);

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

            var rootUri = new Uri(uriString, UriKind.Absolute);

            return rootUri;
        }

        private static FileInfo[] GetSourceFiles(string source)
        {
            FileInfo[] files = null;

            if (!File.Exists(source))
            {
                if (Directory.Exists(Path.GetFullPath(source)))
                {
                    var di = new DirectoryInfo(source);
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
