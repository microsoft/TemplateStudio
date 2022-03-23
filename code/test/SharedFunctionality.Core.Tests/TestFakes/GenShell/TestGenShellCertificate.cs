// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen.Shell;

namespace Microsoft.Templates.Core.Test.TestFakes.GenShell
{
    public class TestGenShellCertificate : IGenShellCertificate
    {
        public string CreateCertificate(string publisherName) => TestCertificateService.Instance.CreateCertificate(publisherName);
    }
}
