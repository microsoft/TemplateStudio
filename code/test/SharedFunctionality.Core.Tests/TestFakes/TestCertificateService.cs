// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Templates.Core.Test.TestFakes
{
    internal class TestCertificateService
    {
        public static TestCertificateService Instance { get; } = new TestCertificateService();

        public string CreateCertificate(string publisherName)
        {
            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={publisherName}");

            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                var certificate = GetCertificateFromRequest(request);

                certificate.FriendlyName = publisherName;

                return Convert.ToBase64String(certificate.Export(X509ContentType.Pfx, string.Empty));
            }
        }

        private X509Certificate2 GetCertificateFromRequest(CertificateRequest request)
        {
            request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));

            request.CertificateExtensions.Add(
               new X509EnhancedKeyUsageExtension(
                   new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));

            return request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
        }
    }
}
