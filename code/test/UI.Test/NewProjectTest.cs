// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;
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
        private const string MvvmToolkit = "MVVMToolkit";

        // Frameworks
        private const string Blank = "Blank";
        private const string MVVMLight = "MVVMLight";

        // Pages
        private const string PageBlank = "wts.Page.Blank";
        private const string PageBlankCodeBehind = "wts.Page.Blank.CodeBehind";
        private const string PageSettings = "wts.Page.Settings";
        private const string PageSettingsCodeBehind = "wts.Page.Settings.CodeBehind";
        private const string PageChartCodeBehind = "wts.Page.Chart.CodeBehind";
        private const string PageChart = "wts.Page.Chart";
        private const string PageGridCodeBehind = "wts.Page.Grid.CodeBehind";
        private const string PageGrid = "wts.Page.Grid";

        // Features
        private const string FeatureSettingsStorage = "wts.Feat.SettingsStorage";

        // Services
        private const string ServiceWebApi = "wts.Service.WebApi";

        private TemplatesFixture _fixture;

        public NewProjectTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(Platforms.Uwp, ProgrammingLanguages.CSharp);
        }

        [Fact]
        public async Task ProjectInitDefaultAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            var pages = viewModel.UserSelection.Groups.First(p => p.TemplateType == TemplateType.Page);
            var features = viewModel.UserSelection.Groups.First(p => p.TemplateType == TemplateType.Feature);
            viewModel.UnsubscribeEventHandlers();

            Assert.Equal(4, viewModel.ProjectType.Items.Count);
            Assert.Equal(6, viewModel.Framework.Items.Count);
            Assert.True(viewModel.StepsViewModels[TemplateType.Page].Groups.Count > 0);
            Assert.True(viewModel.StepsViewModels[TemplateType.Feature].Groups.Count > 0);
            Assert.True(viewModel.StepsViewModels[TemplateType.Service].Groups.Count > 0);
            Assert.True(viewModel.StepsViewModels[TemplateType.Testing].Groups.Count > 0);
            Assert.Single(pages.Items);
            Assert.Empty(features.Items);
        }

        [Fact]
        public async Task ProjectInitUpdatedConfigurationAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Equal(SplitView, userSelection.ProjectType);
            Assert.Equal(MvvmToolkit, userSelection.FrontEndFramework);
            Assert.Equal(PageBlank, userSelection.Pages.First().TemplateId);
            await SetProjectTypeAsync(viewModel, Blank);
            await SetFrameworkAsync(viewModel, MVVMLight);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Equal(Blank, userSelection.ProjectType);
            Assert.Equal(MVVMLight, userSelection.FrontEndFramework);
            Assert.Equal(PageBlank, userSelection.Pages.First().TemplateId);
        }

        [Fact]
        public async Task ResolveDependenciesAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            var settingsTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageSettings);
            var numOfDependencies = settingsTemplate.Dependencies?.Count();
            await AddTemplateAsync(viewModel, settingsTemplate);
            var userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Equal(2, userSelection.Pages.Count);
            Assert.Equal(numOfDependencies, userSelection.Features.Count);
            Assert.Equal(FeatureSettingsStorage, userSelection.Features.First().TemplateId);
        }

        [Fact]
        public async Task ResolveDependenciesAndLicensesAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Single(userSelection.Pages);
            Assert.Empty(userSelection.Features);
            Assert.Equal(2, viewModel.UserSelection.Licenses.Count);
            var settingsTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageSettings);
            var numOfDependencies = settingsTemplate.Dependencies?.Count();
            await AddTemplateAsync(viewModel, settingsTemplate);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Equal(2, userSelection.Pages.Count);
            Assert.Equal(numOfDependencies, userSelection.Features.Count);
            Assert.Equal(3, viewModel.UserSelection.Licenses.Count);
        }

        [Fact]
        public async Task RemovePageAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages.Count == 2);
            DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 1);
            userSelection = viewModel.UserSelection.GetUserSelection();

            viewModel.UnsubscribeEventHandlers();

            Assert.Single(userSelection.Pages);
        }

        [Fact]
        public async Task RemoveTemplateWithHiddenDependencyAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Service].Groups, ServiceWebApi));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Equal(3, userSelection.Services.Count);
            DeleteTemplate(TemplateType.Service, viewModel.UserSelection, 2);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.True(userSelection.Services.Count == 1);
        }

        [Fact]
        public async Task CanNotRemoveHomePageAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);

            await viewModel.OnTemplatesAvailableAsync();

            DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 0);
            var userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Single(userSelection.Pages);
        }

        [Fact]
        public async Task CanNotRemoveTemplateWithDependencyAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.UserSelection.ResetUserSelection();
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            var settingsTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageSettings);
            var numOfDependencies = settingsTemplate.Dependencies?.Count();
            await AddTemplateAsync(viewModel, settingsTemplate);
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Equal(numOfDependencies, userSelection.Features.Count);
            DeleteTemplate(TemplateType.Feature, viewModel.UserSelection, 0);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Equal(numOfDependencies, userSelection.Features.Count);
        }

        [Fact]
        public async Task RemoveHiddenFeaturesAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            var chartTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageChart);
            var gridTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageGrid);
            var numOfDependencies = chartTemplate.Dependencies?.Count();
            await AddTemplateAsync(viewModel, chartTemplate);
            await AddTemplateAsync(viewModel, gridTemplate);

            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Equal(3, userSelection.Pages.Count);
            Assert.Equal(numOfDependencies, userSelection.Services.Count);

            DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 2);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Equal(2, userSelection.Pages.Count);
            Assert.Equal(numOfDependencies, userSelection.Services.Count);

            DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 1);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Single(userSelection.Pages);
            Assert.Equal(numOfDependencies, userSelection.Services.Count + 1);
        }

        [Fact]
        public async Task ReorderPagesUsingKeyboardAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages[1].Name == "Blank");
            Assert.True(userSelection.Pages[2].Name == "Blank1");
            Assert.True(userSelection.Pages[3].Name == "Blank2");
            Assert.True(userSelection.Pages[4].Name == "Blank3");
            var pages = viewModel.UserSelection.Groups.First(g => g.TemplateType == TemplateType.Page);
            pages.EnableOrdering(null);
            pages.SelectedItem = pages.Items[2]; // Select Blank1
            pages.MoveDownCommand.Execute(null);
            userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages[1].Name == "Blank");
            Assert.True(userSelection.Pages[2].Name == "Blank2");
            Assert.True(userSelection.Pages[3].Name == "Blank1");
            Assert.True(userSelection.Pages[4].Name == "Blank3");
            pages.MoveUpCommand.Execute(null);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.True(userSelection.Pages[1].Name == "Blank");
            Assert.True(userSelection.Pages[2].Name == "Blank1");
            Assert.True(userSelection.Pages[3].Name == "Blank2");
            Assert.True(userSelection.Pages[4].Name == "Blank3");
        }

        [Fact]
        public async Task UpdateHomePageAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            viewModel.Initialize(Platforms.Uwp, GenContext.CurrentLanguage);
            await viewModel.OnTemplatesAvailableAsync();

            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.True(userSelection.Pages[0].Name == "Main");
            Assert.True(userSelection.Pages[1].Name == "Blank");
            Assert.True(userSelection.HomeName == "Main");
            var pages = viewModel.UserSelection.Groups.First(g => g.TemplateType == TemplateType.Page);
            pages.EnableOrdering(null);
            pages.SelectedItem = pages.Items[1]; // Select Blank
            pages.MoveUpCommand.Execute(null);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.True(userSelection.Pages[0].Name == "Blank");
            Assert.True(userSelection.Pages[1].Name == "Main");
            Assert.True(userSelection.HomeName == "Blank");
        }

        private async Task SetFrameworkAsync(MainViewModel viewModel, string framework)
        {
            await viewModel.ProcessItemAsync(viewModel.Framework.Items.First(pt => pt.Name == framework));
        }

        private async Task SetProjectTypeAsync(MainViewModel viewModel, string projectType)
        {
            await viewModel.ProcessItemAsync(viewModel.ProjectType.Items.First(pt => pt.Name == projectType));
        }

        private async Task AddTemplateAsync(MainViewModel viewModel, TemplateInfoViewModel template)
        {
            await viewModel.ProcessItemAsync(template);
        }

        private void DeleteTemplate(TemplateType templateType, UserSelectionViewModel userSelection, int index)
        {
            var items = userSelection.Groups.First(g => g.TemplateType == templateType);
            var item = items.Items.ElementAt(index);
            if (item.DeleteCommand.CanExecute(item))
            {
                item.DeleteCommand.Execute(item);
            }
        }

        private TemplateInfoViewModel GetTemplate(ObservableCollection<TemplateGroupViewModel> groups, string templateIdentity)
        {
            var group = groups.First(gr => gr.Items.Any(p => p.Identity == templateIdentity));
            return group.Items.First(t => t.Identity == templateIdentity);
        }
    }
}
