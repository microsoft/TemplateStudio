// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Xunit;

namespace UI.Test
{
    public class LicensesServiceTest : IClassFixture<TemplatesFixture>
    {
        private TemplatesFixture _fixture;

        public LicensesServiceTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(ProgrammingLanguages.CSharp);
        }

        [Fact]
        public void RebuildLicenses_Default()
        {
            var userSelection = new UserSelection()
            {
                ProjectType = "SplitView",
                Framework = "MVVMLight",
                HomeName = "Main"
            };

            ITemplateInfo template = _fixture.Repository.Get(t => t.Identity == "wts.Page.Blank").FirstOrDefault();
            userSelection.Pages.Add(("Main", template));

            var licenses = new List<SummaryLicenseViewModel>();
            LicensesService.RebuildLicenses(userSelection, licenses);

            Assert.True(licenses.Count == 2);
            Assert.True(licenses.Any(l => l.Text == "MVVM Light"));
            Assert.True(licenses.Any(l => l.Text == "Microsoft.Toolkit.Uwp"));
        }

        [Fact]
        public void RebuildLicenses_AddRemovePage()
        {
            var licenses = new List<SummaryLicenseViewModel>();

            licenses.Add(new SummaryLicenseViewModel(new TemplateLicense() { Text = "TestLicense", Url = "Test" }));

            var userSelection = new UserSelection()
            {
                ProjectType = "SplitView",
                Framework = "MVVMLight",
                HomeName = "Main"
            };

            userSelection.Pages.Add(("Main", _fixture.Repository.Get(t => t.Identity == "wts.Page.Blank").FirstOrDefault()));
            userSelection.Features.Add(("SettingStorage", _fixture.Repository.Get(t => t.Identity == "wts.Feat.SettingsStorage").FirstOrDefault()));

            LicensesService.RebuildLicenses(userSelection, licenses);

            Assert.True(licenses.Count == 3);
            Assert.False(licenses.Any(l => l.Text == "TestLicense"));
            Assert.True(licenses.Any(l => l.Text == "MVVM Light"));
            Assert.True(licenses.Any(l => l.Text == "Microsoft.Toolkit.Uwp"));
            Assert.True(licenses.Any(l => l.Text == "Newtonsoft.Json"));
        }
    }
}
