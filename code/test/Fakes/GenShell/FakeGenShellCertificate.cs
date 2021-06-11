// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen.Shell;
using Microsoft.Templates.Utilities.Services;

namespace Microsoft.Templates.Fakes.GenShell
{
    public class FakeGenShellCertificate : IGenShellCertificate
    {
        public string CreateCertificate(string publisherName)
        {
            return CertificateService.Instance.CreateCertificate(publisherName);
        }
    }
}
