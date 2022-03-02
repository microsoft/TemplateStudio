// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Services
{
    public static class LicensesService
    {
        public static void SyncLicenses(IEnumerable<TemplateLicense> genLicenses, IList<LicenseViewModel> licenses)
        {
            var toRemove = new List<LicenseViewModel>();

            foreach (var summaryLicense in licenses)
            {
                if (!genLicenses.Any(l => l.Url == summaryLicense.Url))
                {
                    toRemove.Add(summaryLicense);
                }
            }

            foreach (var licenseToRemove in toRemove)
            {
                licenses.Remove(licenseToRemove);
            }

            foreach (var license in genLicenses)
            {
                if (!licenses.Any(l => l.Url == license.Url))
                {
                    licenses.Add(new LicenseViewModel(license));
                }
            }
        }
    }
}
