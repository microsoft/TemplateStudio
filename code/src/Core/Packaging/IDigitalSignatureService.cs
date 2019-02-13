// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO.Packaging;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Templates.Core.Packaging
{
    public interface IDigitalSignatureService
    {
        X509ChainStatusFlags VerifyCertificate(X509Certificate cert);

        bool IsSigned(Package package);

        IEnumerable<X509Certificate> GetX509Certificates(Package package);

        void SignPackage(Package package, X509Certificate cert);

        bool VerifySignatures(Package package);

        Dictionary<string, X509Certificate> GetPackageCertificates(Package package);

        // TODO WTS: This property is a temporal patch to disable verify signatures.
        // Required as signature can´t be verify in .net core
        bool CanVerifySignatures { get; }
    }
}
