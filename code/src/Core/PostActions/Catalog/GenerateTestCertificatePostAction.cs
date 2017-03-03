using CERTENROLLLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class GenerateTestCertificatePostAction : PostAction<string>
    {
        public GenerateTestCertificatePostAction(GenShell shell, string config) : base(shell, config)
        {
        }

        public override void Execute()
        {
            //TODO: VERIFY THIS OUTSIDE
            //var userName = genInfo.GetUserName();
            //if (string.IsNullOrEmpty(userName))
            //{
            //    throw new Exception(PostActionResources.GenerateTestCertificate_EmptyUserName);
            //}

            var publisherName = _config;
            // create DN for subject and issuer
            var dn = new CX500DistinguishedName();
            dn.Encode("CN=" + publisherName);

            // create a new private key for the certificate
            var privateKey = new CX509PrivateKey()
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
            ku.InitializeEncode(CERTENROLLLib.X509KeyUsageFlags.XCN_CERT_DIGITAL_SIGNATURE_KEY_USAGE);
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
            var request = enrollment.CreateRequest();
            enrollment.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedCertificate, request, EncodingType.XCN_CRYPT_STRING_BASE64, "");
            
            var base64Encoded = enrollment.CreatePFX("", PFXExportOptions.PFXExportChainWithRoot);
            var certificate = new X509Certificate2(Convert.FromBase64String(base64Encoded), "");

            var filePath = Path.Combine(_shell.ProjectPath, _shell.ProjectName) + "_TemporaryKey.pfx";
            File.WriteAllBytes(filePath, Convert.FromBase64String(base64Encoded));

            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);

            store.Remove(certificate);
            store.Close();
            _shell.AddItems(filePath);
        }
    }
}
