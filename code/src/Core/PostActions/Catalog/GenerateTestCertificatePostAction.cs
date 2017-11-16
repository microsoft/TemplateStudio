// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

using CERTENROLLLib;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    // This is a Template Defined Post-Action with the following configuration in the template.
    //   "postActions": [
    //    {
    //        "description": "Generate Test Certificate",
    //        "manualInstructions": [ ],
    //        "actionId": "65057255-BD7B-443C-8180-5D82B9DA9E22",
    //        "args": {
    //            "files" : "0"
    //        },
    //        "continueOnError": "true"
    //    }
    //  ]
    // Expected args:
    //    - files -> reference the file index from the primary outputs which is the target project where the certificate will be added
    // Remarks: this post action can work with single project templates or multi project templates.
    public class GenerateTestCertificatePostAction : TemplateDefinedPostAction
    {
        public static readonly Guid Id = new Guid("65057255-BD7B-443C-8180-5D82B9DA9E22");

        public override Guid ActionId { get => Id; }

        private Dictionary<string, string> _parameters;
        private string _publisherName;
        private IReadOnlyList<ICreationPath> _primaryOutputs;

        public GenerateTestCertificatePostAction(string relatedTemplate, string publisherName, IPostAction templatePostAction, IReadOnlyList<ICreationPath> primaryOutputs, Dictionary<string, string> parameters)
            : base(relatedTemplate, templatePostAction)
        {
            _parameters = parameters;
            _publisherName = publisherName;
            _primaryOutputs = primaryOutputs;
        }

        internal override void ExecuteInternal()
        {
            int targetProjectIndex = int.Parse(Args["files"]);
            var projectFile = _primaryOutputs[targetProjectIndex].GetOutputPath(_parameters);
            string projectFileWithoutExtension = projectFile.Replace(Path.GetExtension(projectFile), string.Empty);

            var pfx = CreateCertificate(_publisherName);

            AddToProject(pfx, projectFileWithoutExtension);
            RemoveFromStore(pfx);
        }

        private void AddToProject(string base64Encoded, string projectFileWithoutExtension)
        {
            var filePath = Path.Combine(GenContext.Current.OutputPath, projectFileWithoutExtension) + "_TemporaryKey.pfx";
            File.WriteAllBytes(filePath, Convert.FromBase64String(base64Encoded));

            GenContext.ToolBox.Shell.AddItems(filePath);
        }

        private static void RemoveFromStore(string base64Encoded)
        {
            var certificate = new X509Certificate2(Convert.FromBase64String(base64Encoded), string.Empty);
            var store = new X509Store(StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadWrite);
            store.Remove(certificate);
            store.Close();
        }

        private string CreateCertificate(string publisherName)
        {
            var cert = CreateCertificateRequest(publisherName);

            AddKeyUsage(cert);
            AddExtendedKeyUsage(cert);
            AddBasicConstraints(cert);

            // Specify the hashing algorithm
            var hashobj = new CObjectId();

            hashobj.InitializeFromAlgorithmName(
                ObjectIdGroupId.XCN_CRYPT_HASH_ALG_OID_GROUP_ID,
                ObjectIdPublicKeyFlags.XCN_CRYPT_OID_INFO_PUBKEY_ANY,
                AlgorithmFlags.AlgorithmFlagsNone,
                "SHA256");

            cert.HashAlgorithm = hashobj;
            cert.Encode();

            // Do the final enrollment process
            var enrollment = new CX509Enrollment();

            enrollment.InitializeFromRequest(cert); // load the certificate
            enrollment.CertificateFriendlyName = cert.Subject.Name;

            var request = enrollment.CreateRequest();

            enrollment.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedCertificate, request, EncodingType.XCN_CRYPT_STRING_BASE64, string.Empty);

            var base64Encoded = enrollment.CreatePFX(string.Empty, PFXExportOptions.PFXExportChainWithRoot);

            return base64Encoded;
        }

        private void AddBasicConstraints(CX509CertificateRequestCertificate cert)
        {
            // Add basic constraints
            var bc = new CX509ExtensionBasicConstraints();

            bc.InitializeEncode(false, 0);
            bc.Critical = true;

            cert.X509Extensions.Add((CX509Extension)bc);
        }

        private void AddExtendedKeyUsage(CX509CertificateRequestCertificate cert)
        {
            // Add extended key usage
            var eku = new CX509ExtensionEnhancedKeyUsage();
            var oid = new CObjectId();

            oid.InitializeFromValue("1.3.6.1.5.5.7.3.3");
            eku.InitializeEncode(new CObjectIds() { oid });

            eku.Critical = true;

            cert.X509Extensions.Add((CX509Extension)eku);
        }

        private void AddKeyUsage(CX509CertificateRequestCertificate cert)
        {
            // Add key usage
            var ku = new CX509ExtensionKeyUsage();
            ku.InitializeEncode(CERTENROLLLib.X509KeyUsageFlags.XCN_CERT_DIGITAL_SIGNATURE_KEY_USAGE);

            ku.Critical = false;

            cert.X509Extensions.Add((CX509Extension)ku);
        }

        private CX509CertificateRequestCertificate CreateCertificateRequest(string publisherName)
        {
            // create DN for subject and issuer
            var dn = new CX500DistinguishedName();
            dn.Encode("CN=" + publisherName);

            // create a new private key for the certificate
            var privateKey = (IX509PrivateKey)Activator.CreateInstance(Type.GetTypeFromProgID("X509Enrollment.CX509PrivateKey"));

            privateKey.ProviderName = "Microsoft Base Cryptographic Provider v1.0";
            privateKey.MachineContext = false;
            privateKey.Length = 2048;
            privateKey.KeySpec = X509KeySpec.XCN_AT_SIGNATURE;
            privateKey.KeyUsage = X509PrivateKeyUsageFlags.XCN_NCRYPT_ALLOW_SIGNING_FLAG;
            privateKey.ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_PLAINTEXT_EXPORT_FLAG;
            privateKey.Create();

            // Create the self signing request
            var cert = new CX509CertificateRequestCertificate();

            cert.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextUser, privateKey, string.Empty);
            cert.Subject = dn;
            cert.Issuer = dn;
            cert.NotBefore = DateTime.Now.Date.AddDays(-1);
            cert.NotAfter = DateTime.Now.Date.AddYears(1);

            return cert;
        }
    }
}
