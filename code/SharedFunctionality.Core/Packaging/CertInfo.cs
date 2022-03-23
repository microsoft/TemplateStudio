// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Security.Cryptography.X509Certificates;

namespace Microsoft.Templates.Core.Packaging
{
    public class CertInfo
    {
        public X509Certificate2 Cert { get; set; }

        public string Pin { get; set; }

        public X509ChainStatusFlags Status { get; set; }
    }
}
