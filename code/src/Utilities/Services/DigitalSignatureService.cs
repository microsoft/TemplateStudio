// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Utilities.Services
{
    public class DigitalSignatureService : IDigitalSignatureService
    {
        public bool CanVerifySignatures => true;

        public X509ChainStatusFlags VerifyCertificate(X509Certificate cert)
        {
            return PackageDigitalSignatureManager.VerifyCertificate(cert);
        }

        public bool IsSigned(Package package)
        {
            var dsm = new PackageDigitalSignatureManager(package);
            return dsm.IsSigned;
        }

        public IEnumerable<X509Certificate> GetX509Certificates(Package package)
        {
            var certs = GetPackageCertificates(package);
            return certs.Values;
        }

        public void SignPackage(Package package, X509Certificate cert)
        {
            var dsm = new PackageDigitalSignatureManager(package)
            {
                CertificateOption = CertificateEmbeddingOption.InSignaturePart,
                HashAlgorithm = SignedXml.XmlDsigSHA512Url,
            };

            var toSign = new List<Uri>();

            foreach (PackagePart packagePart in package.GetParts())
            {
                toSign.Add(packagePart.Uri);
            }

            toSign.Add(PackUriHelper.GetRelationshipPartUri(dsm.SignatureOrigin));
            toSign.Add(dsm.SignatureOrigin);
            toSign.Add(PackUriHelper.GetRelationshipPartUri(new Uri("/", UriKind.RelativeOrAbsolute)));

            dsm.Sign(toSign, cert);
        }

        public bool VerifySignatures(Package package)
        {
            var dsm = new PackageDigitalSignatureManager(package);
            VerifyResult verifyResult = dsm.VerifySignatures(false);
            return verifyResult == VerifyResult.Success;
        }

        public Dictionary<string, X509Certificate> GetPackageCertificates(Package package)
        {
            var dsm = new PackageDigitalSignatureManager(package);

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
    }
}
