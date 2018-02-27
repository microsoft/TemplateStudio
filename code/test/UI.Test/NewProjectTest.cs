// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.Templates.UI.Views.NewProject;
using Xunit;

namespace Microsoft.UI.Test
{
    [Collection("UI")]
    [Trait("ExecutionSet", "Minimum")]
    public class NewProjectTest : IClassFixture<TemplatesFixture>
    {
        // Project Types
        private const string SplitView = "SplitView";
        private const string CodeBehind = "CodeBehind";

        // Frameworks
        private const string Blank = "Blank";
        private const string MVVMLight = "MVVMLight";

        // Pages
        private const string PageBlank = "wts.Page.Blank";
        private const string PageBlankCodeBehind = "wts.Page.Blank.CodeBehind";
        private const string PageSettings = "wts.Page.Settings";
        private const string PageSettingsCodeBehind = "wts.Page.Settings.CodeBehind";
        private const string PageChartCodeBehind = "wts.Page.Chart.CodeBehind";
        private const string PageGridCodeBehind = "wts.Page.Grid.CodeBehind";

        // Features
        private const string FeatureSettingsStorage = "wts.Feat.SettingsStorage";

        private TemplatesFixture _fixture;

        public NewProjectTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(ProgrammingLanguages.CSharp);
        }

        [Fact]
        public async Task ProjectInitDefaultAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);

            Assert.True(viewModel.ProjectType.Items.Count == 3);
            Assert.True(viewModel.Framework.Items.Count == 5);
            Assert.True(viewModel.AddPages.Groups.Count > 0);
            Assert.True(viewModel.AddFeatures.Groups.Count > 0);
            Assert.True(viewModel.UserSelection.Pages.Count == 1);
            Assert.True(viewModel.UserSelection.Features.Count == 0);
        }

        [Fact]
        public async Task ProjectInitUpdatedConfigurationAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);

            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.ProjectType == SplitView);
            Assert.True(userSelection.Framework == CodeBehind);
            Assert.True(userSelection.Pages.First().template.Identity == PageBlankCodeBehind);
            SetProjectType(viewModel, Blank);
            SetFramework(viewModel, MVVMLight);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.ProjectType == Blank);
            Assert.True(userSelection.Framework == MVVMLight);
            Assert.True(userSelection.Pages.First().template.Identity == PageBlank);
        }

        [Fact]
        public async Task ResolveDependenciesAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageSettingsCodeBehind));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 2);
            Assert.True(userSelection.Features.Count == 1);
            Assert.True(userSelection.Features.First().template.Identity == FeatureSettingsStorage);
        }

        [Fact]
        public async Task ResolveDependenciesAndLicensesAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 1);
            Assert.True(userSelection.Features.Count == 0);
            Assert.True(viewModel.UserSelection.Licenses.Count == 1);
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageSettingsCodeBehind));
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 2);
            Assert.True(userSelection.Features.Count == 1);
            Assert.True(viewModel.UserSelection.Licenses.Count == 2);
        }

        [Fact]
        public async Task RemovePageAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageBlankCodeBehind));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 2);
            RemoveTemplate(viewModel.UserSelection.Pages, 1);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 1);
        }

        [Fact]
        public async Task CanNotRemoveHomePageAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);
            RemoveTemplate(viewModel.UserSelection.Pages, 0);
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 1);
        }

        [Fact]
        public async Task CanNotRemoveTemplateWithDependencyAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageSettingsCodeBehind));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Features.Count == 1);
            RemoveTemplate(viewModel.UserSelection.Features, 0);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Features.Count == 1);
        }

        [Fact]
        public async Task RemoveHiddenFeaturesAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageChartCodeBehind));
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageGridCodeBehind));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 3);
            Assert.True(userSelection.Features.Count == 1);
            RemoveTemplate(viewModel.UserSelection.Pages, 2);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 2);
            Assert.True(userSelection.Features.Count == 1);
            RemoveTemplate(viewModel.UserSelection.Pages, 1);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 1);
            Assert.True(userSelection.Features.Count == 0);
        }

        [Fact]
        public async Task ReorderPagesUsingKeyboardAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageBlankCodeBehind));
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageBlankCodeBehind));
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageBlankCodeBehind));
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageBlankCodeBehind));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages[1].name == "Blank");
            Assert.True(userSelection.Pages[2].name == "Blank1");
            Assert.True(userSelection.Pages[3].name == "Blank2");
            Assert.True(userSelection.Pages[4].name == "Blank3");
            viewModel.UserSelection.SelectedPage = viewModel.UserSelection.Pages[2]; // Select Blank1
            viewModel.UserSelection.MovePageDownCommand.Execute(null);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages[1].name == "Blank");
            Assert.True(userSelection.Pages[2].name == "Blank2");
            Assert.True(userSelection.Pages[3].name == "Blank1");
            Assert.True(userSelection.Pages[4].name == "Blank3");
            viewModel.UserSelection.MovePageUpCommand.Execute(null);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages[1].name == "Blank");
            Assert.True(userSelection.Pages[2].name == "Blank1");
            Assert.True(userSelection.Pages[3].name == "Blank2");
            Assert.True(userSelection.Pages[4].name == "Blank3");
        }

        [Fact]
        public async Task UpdateHomePageAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel(null, new UITestStyleValuesProvider());
            await viewModel.InitializeAsync(GenContext.CurrentLanguage);
            AddTemplate(viewModel, GetTemplate(viewModel.AddPages.Groups, PageBlankCodeBehind));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages[0].name == "Main");
            Assert.True(userSelection.Pages[1].name == "Blank");
            Assert.True(userSelection.HomeName == "Main");
            viewModel.UserSelection.SelectedPage = viewModel.UserSelection.Pages[1]; // Select Blank
            viewModel.UserSelection.MovePageUpCommand.Execute(null);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages[0].name == "Blank");
            Assert.True(userSelection.Pages[1].name == "Main");
            Assert.True(userSelection.HomeName == "Blank");
        }

        private void SetFramework(MainViewModel viewModel, string framework)
        {
            viewModel.ProcessItem(viewModel.Framework.Items.First(pt => pt.Name == framework));
        }

        private void SetProjectType(MainViewModel viewModel, string projectType)
        {
            viewModel.ProcessItem(viewModel.ProjectType.Items.First(pt => pt.Name == projectType));
        }

        private void AddTemplate(MainViewModel viewModel, TemplateInfoViewModel template)
        {
            viewModel.ProcessItem(template);
        }

        private void RemoveTemplate(ObservableCollection<SavedTemplateViewModel> templates, int index)
        {
            templates.ElementAt(index).DeleteCommand.Execute(null);
        }

        private TemplateInfoViewModel GetTemplate(ObservableCollection<TemplateGroupViewModel> groups, string templateIdentity)
        {
            var group = groups.First(gr => gr.Items.Any(p => p.Identity == templateIdentity));
            return group.Items.First(t => t.Identity == templateIdentity);
        }
    }
}
