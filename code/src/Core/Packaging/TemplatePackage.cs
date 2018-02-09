// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Packaging
{
    public static class TemplatePackage
    {
        public const string DefaultExtension = ".mstx";

        private const int BufSize = 0x1000;
        private const string TemplatesContentRelationshipType = "http://schemas.microsoft.com/opc/2006/06/templates/securecontent";

        private static string CreateSourcePath(string source)
        {
            return source + DefaultExtension;
        }

        public static async Task<string> PackAndSignAsync(string source, string certThumbprint, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string outFile = CreateSourcePath(source);

            await PackAndSignAsync(source, outFile, certThumbprint, mimeMediaType);

            return outFile;
        }

        public static async Task<string> PackAndSignAsync(string source, X509Certificate signingCert, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string outFile = CreateSourcePath(source);

            await PackAndSignAsync(source, outFile, signingCert, mimeMediaType);

            return outFile;
        }

        public static async Task PackAndSignAsync(string source, string outFile, string certThumbprint, string mimeMediaType)
        {
            X509Certificate cert = LoadCert(certThumbprint);

            if (cert == null)
            {
                throw new SignCertNotFoundException(string.Format(StringRes.TemplatePackagePackAndSignMessage, certThumbprint));
            }

            await PackAndSignAsync(source, outFile, cert, mimeMediaType);
        }

        public static async Task PackAndSignAsync(string source, string outFile, X509Certificate signingCert, string mimeMediaType)
        {
            await PackAsync(source, outFile, mimeMediaType);
            Sign(outFile, signingCert);
        }

        public static async Task<string> PackAsync(string source, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string outFile = source + DefaultExtension;

            await PackAsync(source, outFile, mimeMediaType);

            return outFile;
        }

        public static async Task PackAsync(string source, string outFile, string mimeMediaType)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentException("source");
            }

            if (string.IsNullOrWhiteSpace(outFile))
            {
                throw new ArgumentException("outFile");
            }

            FileInfo[] files = GetSourceFiles(source);
            Uri rootUri = GetRootUri(source);

            EnsureDirectory(Path.GetDirectoryName(outFile));

            using (Package package = Package.Open(outFile, FileMode.Create))
            {
                foreach (var file in files)
                {
                    Uri partUriFile = GetPartUriFile(rootUri, file);
                    PackagePart packagePart = package.CreatePart(partUriFile, mimeMediaType, CompressionOption.Maximum);

                    await AddContentToPackagePartAsync(file, packagePart);

                    package.CreateRelationship(packagePart.Uri, TargetMode.Internal, TemplatesContentRelationshipType);
                }

                package.Flush();
            }
        }

        public static async Task ExtractAsync(string signedFilePack, string targetDirectory, bool verifySignatures = true, Action<int> reportProgress = null, CancellationToken ct = default(CancellationToken))
        {
            string currentDir = Environment.CurrentDirectory;
            string inFilePack = Path.IsPathRooted(signedFilePack) ? signedFilePack : Path.Combine(currentDir, signedFilePack);
            string outDir = Path.IsPathRooted(targetDirectory) ? targetDirectory : Path.Combine(currentDir, targetDirectory);

            EnsureDirectory(outDir);

            if (!ct.IsCancellationRequested)
            {
                using (Package package = Package.Open(inFilePack, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    bool isSignatureValid = false;

                    if (verifySignatures)
                    {
                        isSignatureValid = ValidateSignatures(package);
                    }

                    if (isSignatureValid || !verifySignatures)
                    {
                        await ExtractContentAsync(outDir, package, reportProgress, ct);
                    }

                    if (!isSignatureValid && verifySignatures)
                    {
                        throw new InvalidSignatureException(string.Format(StringRes.TemplatePackageExtractMessage, signedFilePack));
                    }
                }
            }
        }

        private static void Sign(string file, X509Certificate signingCert)
        {
            if (signingCert == null)
            {
                throw new ArgumentException("signingCert");
            }

            using (Package package = Package.Open(file, FileMode.Open))
            {
                SignAllParts(package, signingCert);
            }
        }

        private static async Task ExtractContentAsync(string outDir, Package package, Action<int> reportProgress, CancellationToken ct)
        {
            PackagePart packagePartDocument = null;
            int partCounter = 0;
            int totalParts = package.GetRelationshipsByType(TemplatesContentRelationshipType).Count();
            int latestProgress = 0;

            foreach (PackageRelationship relationship in package.GetRelationshipsByType(TemplatesContentRelationshipType))
            {
                ct.ThrowIfCancellationRequested();
                partCounter++;

                var progress = Convert.ToInt32((partCounter * 100) / totalParts);
                if (progress != latestProgress)
                {
                    reportProgress?.Invoke(progress);
                    latestProgress = progress;
                }

                // Open the Document part, write the contents to a file.
                packagePartDocument = package.GetPart(relationship.TargetUri);
                await ExtractPartAsync(packagePartDocument, outDir);
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
                AppHealth.Current.Warning.TrackAsync(string.Format(StringRes.TemplatePackageLoadCertMessage, thumbprint)).FireAndForget();
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

                AppHealth.Current.Info.TrackAsync(string.Format(StringRes.TemplatePackageFindCertificateFoundMessage, certs.Count, thumbprint, location.ToString())).FireAndForget();

                if (certs.Count >= 1)
                {
                    certFound = certs[0];

                    if (certs.Count > 1)
                    {
                        AppHealth.Current.Warning.TrackAsync(StringRes.TemplatePackageFindCertificateNotOneMessage).FireAndForget();
                    }

                    if (!certFound.HasPrivateKey)
                    {
                        AppHealth.Current.Info.TrackAsync(StringRes.TemplatePackageFindCertificateNoPkMessage).FireAndForget();
                    }
                }
            }

            return certFound;
        }

        private static async Task ExtractPartAsync(PackagePart packagePart, string targetDirectory)
        {
            string stringPart = packagePart.Uri.ToString().TrimStart('/');
            var partUri = new Uri(stringPart, UriKind.Relative);
            string dir = targetDirectory.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase) ? targetDirectory : targetDirectory + Path.DirectorySeparatorChar;
            var uriFullPartPath = new Uri(new Uri(dir, UriKind.Absolute), partUri);

            // When packing, directories are automatically converted when turned into a URI
            // Decode to preserve any spaces in directory names (such as `My Project` in a VB app)
            Directory.CreateDirectory(System.Net.WebUtility.UrlDecode(Path.GetDirectoryName(uriFullPartPath.LocalPath)));

            using (var fileStream = new FileStream(System.Net.WebUtility.UrlDecode(uriFullPartPath.LocalPath), FileMode.Create))
            {
                await CopyStreamAsync(packagePart.GetStream(), fileStream);
            }
        }

        public static List<(X509Certificate2 cert, string pin, X509ChainStatusFlags status)> GetCertsInfo(string signedPackageFilename)
        {
            using (Package package = Package.Open(signedPackageFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var res = new List<(X509Certificate2 cert, string pin, X509ChainStatusFlags status)>();
                var dsm = new PackageDigitalSignatureManager(package);
                var certs = GetPackageCertificates(dsm);
                foreach (X509Certificate cert in certs.Values)
                {
                    (X509Certificate2 cert, string pin, X509ChainStatusFlags status) certInfo =
                        (cert: new X509Certificate2(cert), pin: cert.GetPublicKeyString().ObfuscateSHA(), PackageDigitalSignatureManager.VerifyCertificate(cert));

                    res.Add(certInfo);
                }

                return res;
            }
        }

        public static bool IsSigned(string signedPackageFilename)
        {
            using (Package package = Package.Open(signedPackageFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var dsm = new PackageDigitalSignatureManager(package);
                return dsm.IsSigned;
            }
        }

        public static bool ValidateSignatures(string signedPackageFilename)
        {
            using (Package package = Package.Open(signedPackageFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ValidateSignatures(package);
            }
        }

        private static bool ValidateSignatures(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("ValidateSignatures(package)");
            }

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
                    AppHealth.Current.Warning.TrackAsync(StringRes.TemplatePackageValidatePackageCertificatesMessage).FireAndForget();
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
            AppHealth.Current.Verbose.TrackAsync(string.Format(StringRes.TemplatePackageVerifyCertificateMessage, cert.Subject, status.ToString())).FireAndForget();

            return status == X509ChainStatusFlags.NoError;
        }

        private static bool VerifyAllowedPublicKey(X509Certificate cert)
        {
            var pubKeyCert = cert.GetPublicKeyString();

            var pubKeyPin = pubKeyCert.ObfuscateSHA();

            AppHealth.Current.Verbose.TrackAsync($"{StringRes.PackageCertificateString} {cert.Subject}").FireAndForget();
            AppHealth.Current.Verbose.TrackAsync($"Key: {pubKeyCert}").FireAndForget();
            AppHealth.Current.Verbose.TrackAsync($"Pin: {pubKeyPin}").FireAndForget();

            bool pinAllowed = Configuration.Current.AllowedPublicKeysPins.Where(allowedPin => allowedPin.Equals(pubKeyPin)).Any();
            AppHealth.Current.Verbose.TrackAsync($"Pin is allowed: {pinAllowed}").FireAndForget();

            return pinAllowed;
        }

        private static void SignAllParts(Package package, X509Certificate cert)
        {
            if (package == null)
            {
                throw new ArgumentNullException("SignAllParts(package)");
            }

            if (cert == null)
            {
                throw new ArgumentNullException("SignAllParts(cert)");
            }

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
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatePackageSignAllPartsMessage, ex).FireAndForget();
                throw;
            }
        }

        private static async Task CopyStreamAsync(Stream source, Stream target)
        {
            var buf = new byte[BufSize];
            int bytesRead = 0;

            while ((bytesRead = await source.ReadAsync(buf, 0, BufSize)) > 0)
            {
                await target.WriteAsync(buf, 0, bytesRead);
            }
        }

        private static void EnsureDirectory(string dir)
        {
            if (!string.IsNullOrEmpty(dir) && !dir.Equals(Environment.CurrentDirectory, StringComparison.OrdinalIgnoreCase))
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        private static async Task AddContentToPackagePartAsync(FileInfo file, PackagePart packagePart)
        {
            using (var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                await CopyStreamAsync(fileStream, packagePart.GetStream());
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
                uriString = Path.GetDirectoryName(Path.GetFullPath(source)).Replace(source, string.Empty);
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
                throw new FileNotFoundException(string.Format(StringRes.TemplatePackageGetSourceFilesMessage, source));
            }

            return files;
        }
    }
}
