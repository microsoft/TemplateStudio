using System;
using System.IO;
using CERTENROLLLib;

namespace Microsoft.Templates.Wizard
{
    public static class CodeSigningCertificate
    {
        public static void CreateTestCertificate(string publisherName, string filePath)
        {
            // create DN for subject and issuer
            var dn = new CX500DistinguishedName();
            dn.Encode("CN=" + publisherName, X500NameFlags.XCN_CERT_NAME_STR_NONE);

            // create a new private key for the certificate
            CX509PrivateKey privateKey = new CX509PrivateKey()
            {
                ProviderName = "Microsoft Base Cryptographic Provider v1.0",
                MachineContext = false,
                Length = 2048,
                KeySpec = X509KeySpec.XCN_AT_SIGNATURE,
                KeyUsage = X509PrivateKeyUsageFlags.XCN_NCRYPT_ALLOW_SIGNING_FLAG,
                ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG
            };
            privateKey.Create();

            // Create the self signing request
            var cert = new CX509CertificateRequestCertificate();
            cert.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextUser, privateKey, "");
            cert.Subject = dn;
            cert.Issuer = dn;
            cert.NotBefore = DateTime.Now.Date.AddDays(-1);
            cert.NotAfter = DateTime.Now.Date.AddYears(1);

            //Add key usage
            var ku = new CX509ExtensionKeyUsage();
            ku.InitializeEncode(X509KeyUsageFlags.XCN_CERT_DIGITAL_SIGNATURE_KEY_USAGE);
            ku.Critical = false;
            cert.X509Extensions.Add((CX509Extension)ku);

            //Add extended key usage 
            var eku = new CX509ExtensionEnhancedKeyUsage();
            var oid = new CObjectId();
            oid.InitializeFromValue("1.3.6.1.5.5.7.3.3");
            eku.InitializeEncode(new CObjectIds() { oid });
            eku.Critical = true;
            cert.X509Extensions.Add((CX509Extension)eku);

            //Add basic constraints
            var bc = new CX509ExtensionBasicConstraints();
            bc.InitializeEncode(false, 0);
            bc.Critical = true;
            cert.X509Extensions.Add((CX509Extension)bc);

            //Specify the hashing algorithm
            var hashobj = new CObjectId();
            hashobj.InitializeFromAlgorithmName(ObjectIdGroupId.XCN_CRYPT_HASH_ALG_OID_GROUP_ID,
                ObjectIdPublicKeyFlags.XCN_CRYPT_OID_INFO_PUBKEY_ANY,
                AlgorithmFlags.AlgorithmFlagsNone, "SHA256");

            cert.HashAlgorithm = hashobj;
            cert.Encode();

            // Do the final enrollment process
            var enrollment = new CX509Enrollment();
            enrollment.InitializeFromRequest(cert); // load the certificate
            enrollment.CertificateFriendlyName = publisherName;
            string request = enrollment.CreateRequest();
            enrollment.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedCertificate, request, EncodingType.XCN_CRYPT_STRING_BASE64, "");

            var base64encoded = enrollment.CreatePFX("", PFXExportOptions.PFXExportChainWithRoot);

            File.WriteAllBytes(filePath, Convert.FromBase64String(base64encoded));
        }
    }
}
