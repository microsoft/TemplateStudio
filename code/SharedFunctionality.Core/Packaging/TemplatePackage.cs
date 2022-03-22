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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Resources;

namespace Microsoft.Templates.Core.Packaging
{
    public class TemplatePackage
    {
        public const string DefaultExtension = ".mstx";

        private const int BufSize = 0x1000;
        private const string TemplatesContentRelationshipType = "http://schemas.microsoft.com/opc/2006/06/templates/securecontent";

        public TemplatePackage(IDigitalSignatureService digitalSignatureService)
        {
            _digitalSignatureService = digitalSignatureService;
        }

        private readonly IDigitalSignatureService _digitalSignatureService;

        private string CreateSourcePath(string source)
        {
            return source + DefaultExtension;
        }

        public async Task<string> PackAndSignAsync(string source, X509Certificate signingCert, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string outFile = CreateSourcePath(source);

            await PackAndSignAsync(source, outFile, signingCert, mimeMediaType);

            return outFile;
        }

        public async Task PackAndSignAsync(string source, string outFile, string certThumbprint, string mimeMediaType)
        {
            X509Certificate cert = LoadCert(certThumbprint);

            if (cert == null)
            {
                throw new SignCertNotFoundException(string.Format(StringRes.TemplatePackagePackAndSignMessage, certThumbprint));
            }

            await PackAndSignAsync(source, outFile, cert, mimeMediaType);
        }

        public async Task PackAndSignAsync(string source, string outFile, X509Certificate signingCert, string mimeMediaType)
        {
            await PackAsync(source, outFile, mimeMediaType);
            Sign(outFile, signingCert);
        }

        public async Task<string> PackAsync(string source, string mimeMediaType = MediaTypeNames.Text.Plain)
        {
            string outFile = source + DefaultExtension;

            await PackAsync(source, outFile, mimeMediaType);

            return outFile;
        }

        public async Task PackAsync(string source, string outFile, string mimeMediaType)
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

        public async Task ExtractAsync(string signedFilePack, string targetDirectory, Action<int> reportProgress = null, CancellationToken ct = default)
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
                    bool verifySignatures = _digitalSignatureService.CanVerifySignatures;

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

        private void Sign(string file, X509Certificate signingCert)
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

        private async Task ExtractContentAsync(string outDir, Package package, Action<int> reportProgress, CancellationToken ct)
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

        public X509Certificate2 LoadCert(string filePath, SecureString password)
        {
            var cert = new X509Certificate2(File.ReadAllBytes(filePath), password, X509KeyStorageFlags.DefaultKeySet);

            return cert;
        }

        public X509Certificate2 LoadCert(string thumbprint)
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

        private X509Certificate2 FindCertificate(string thumbprint, StoreLocation location)
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

        private async Task ExtractPartAsync(PackagePart packagePart, string targetDirectory)
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

        public List<CertInfo> GetCertsInfo(string signedPackageFilename)
        {
            using (Package package = Package.Open(signedPackageFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var res = new List<CertInfo>();
                var certs = _digitalSignatureService.GetX509Certificates(package);
                foreach (X509Certificate cert in certs)
                {
                    var certInfo = new CertInfo()
                    {
                           Cert = new X509Certificate2(cert),
                           Pin = Obfuscate(cert.GetPublicKeyString()),
                           Status = _digitalSignatureService.VerifyCertificate(cert),
                    };

                    res.Add(certInfo);
                }

                return res;
            }
        }

        public bool IsSigned(string signedPackageFilename)
        {
            using (Package package = Package.Open(signedPackageFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return _digitalSignatureService.IsSigned(package);
            }
        }

        public bool ValidateSignatures(string signedPackageFilename)
        {
            using (Package package = Package.Open(signedPackageFilename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ValidateSignatures(package);
            }
        }

        private bool ValidateSignatures(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("ValidateSignatures(package)");
            }

            return _digitalSignatureService.IsSigned(package)
                && ValidatePackageCertificates(package)
                && _digitalSignatureService.VerifySignatures(package);
        }

        private bool ValidatePackageCertificates(Package package)
        {
            Dictionary<string, X509Certificate> certs = _digitalSignatureService.GetPackageCertificates(package);
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

        private bool CertificateChainVaidationRequired()
        {
            return !Configuration.Current.AllowedPublicKeysPins.Where(pk => !string.IsNullOrWhiteSpace(pk)).Any();
        }

        private bool VerifyCertificate(X509Certificate cert)
        {
            var status = _digitalSignatureService.VerifyCertificate(cert);
            AppHealth.Current.Verbose.TrackAsync(string.Format(StringRes.TemplatePackageVerifyCertificateMessage, cert.Subject, status.ToString())).FireAndForget();

            return status == X509ChainStatusFlags.NoError;
        }

        private static string GetHash(HashAlgorithm md5Hash, byte[] inputData)
        {
            byte[] data = md5Hash.ComputeHash(inputData);

            var sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }

            return sb.ToString();
        }

        private static string Obfuscate(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return string.Empty;
            }

            string result = data;
            byte[] b64data = Encoding.UTF8.GetBytes(data);

            using (SHA512 sha2 = SHA512.Create())
            {
                result = GetHash(sha2, b64data);
            }

            return result.ToUpperInvariant();
        }

        private bool VerifyAllowedPublicKey(X509Certificate cert)
        {
            var pubKeyCert = cert.GetPublicKeyString();

            var pubKeyPin = Obfuscate(pubKeyCert);

            AppHealth.Current.Verbose.TrackAsync($"{StringRes.PackageCertificateString} {cert.Subject}").FireAndForget();
            AppHealth.Current.Verbose.TrackAsync($"Key: {pubKeyCert}").FireAndForget();
            AppHealth.Current.Verbose.TrackAsync($"Pin: {pubKeyPin}").FireAndForget();

            bool pinAllowed = Configuration.Current.AllowedPublicKeysPins.Where(allowedPin => allowedPin.Equals(pubKeyPin, StringComparison.Ordinal)).Any();
            AppHealth.Current.Verbose.TrackAsync($"Pin is allowed: {pinAllowed}").FireAndForget();

            return pinAllowed;
        }

        private void SignAllParts(Package package, X509Certificate cert)
        {
            if (package == null)
            {
                throw new ArgumentNullException("SignAllParts(package)");
            }

            if (cert == null)
            {
                throw new ArgumentNullException("SignAllParts(cert)");
            }

            try
            {
                _digitalSignatureService.SignPackage(package, cert);
            }
            catch (CryptographicException ex)
            {
                AppHealth.Current.Error.TrackAsync(StringRes.TemplatePackageSignAllPartsMessage, ex).FireAndForget();
                throw;
            }
        }

        private async Task CopyStreamAsync(Stream source, Stream target)
        {
            var buf = new byte[BufSize];
            int bytesRead = 0;

            while ((bytesRead = await source.ReadAsync(buf, 0, BufSize)) > 0)
            {
                await target.WriteAsync(buf, 0, bytesRead);
            }
        }

        private void EnsureDirectory(string dir)
        {
            if (!string.IsNullOrEmpty(dir) && !dir.Equals(Environment.CurrentDirectory, StringComparison.OrdinalIgnoreCase))
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        private async Task AddContentToPackagePartAsync(FileInfo file, PackagePart packagePart)
        {
            using (var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                await CopyStreamAsync(fileStream, packagePart.GetStream());
            }
        }

        private Uri GetPartUriFile(Uri rootUri, FileInfo file)
        {
            var uriFile = rootUri.MakeRelativeUri(new Uri(file.FullName, UriKind.Absolute));
            var partUriFile = PackUriHelper.CreatePartUri(uriFile);

            return partUriFile;
        }

        private Uri GetRootUri(string source)
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

        private FileInfo[] GetSourceFiles(string source)
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
