// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Services
{
    public class LicensesService : Observable
    {
        private bool _hasSummaryLicenses;
        public bool HasSummaryLicenses
        {
            get => _hasSummaryLicenses;
            private set => SetProperty(ref _hasSummaryLicenses, value);
        }

        public ObservableCollection<SummaryLicenseViewModel> SummaryLicenses { get; } = new ObservableCollection<SummaryLicenseViewModel>();
        Func<UserSelection> GetUserSelection { get; }

        public LicensesService(Func<UserSelection> getUserSelection)
        {
            SummaryLicenses.CollectionChanged += (s, o) => { OnPropertyChanged(nameof(SummaryLicenses)); };
            GetUserSelection = getUserSelection;
        }

        public void RebuildLicenses()
        {
            var genItems = GenComposer.Compose(GetUserSelection());

            var genLicenses = genItems
                                .SelectMany(s => s.Template.GetLicenses())
                                .Distinct(new TemplateLicenseEqualityComparer())
                                .ToList();

            SyncLicenses(genLicenses);
        }

        private void SyncLicenses(IEnumerable<TemplateLicense> licenses)
        {
            var toRemove = new List<SummaryLicenseViewModel>();

            foreach (var summaryLicense in SummaryLicenses)
            {
                if (!licenses.Any(l => l.Url == summaryLicense.Url))
                {
                    toRemove.Add(summaryLicense);
                }
            }

            foreach (var licenseToRemove in toRemove)
            {
                SummaryLicenses.Remove(licenseToRemove);
            }

            foreach (var license in licenses)
            {
                if (!SummaryLicenses.Any(l => l.Url == license.Url))
                {
                    SummaryLicenses.Add(new SummaryLicenseViewModel(license));
                }
            }

            HasSummaryLicenses = SummaryLicenses.Any();
        }
    }
}
