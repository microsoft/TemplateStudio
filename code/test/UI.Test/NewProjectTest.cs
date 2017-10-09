// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.Templates.UI.ViewModels.Common;

using Xunit;
using Microsoft.Templates.UI.Services;

namespace Microsoft.UI.Test
{
    [Collection("UI")]
    [Trait("ExecutionSet", "Minimum")]
    public class NewProjectTest : IClassFixture<TemplatesFixture>
    {
        private TemplatesFixture _fixture;
        public NewProjectTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture("C#");
        }

        [Fact]
        public async Task ProjectInitDefaultAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            await viewModel.ProjectTemplates.InitializeAsync();
            Assert.True(viewModel.ProjectTemplates.SavedPages.Count == 1);
            Assert.True(viewModel.ProjectTemplates.SavedPages.First().Count == 1);
            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 0);
        }

        [Fact]
        public async Task ProjectInitUpdatedConfigurationAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            // Update project to Blank and framework to MVVM Light
            viewModel.ProjectSetup.SelectedProjectType = viewModel.ProjectSetup.ProjectTypes.First(pt => pt.Name == "Blank");
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == "MVVMLight");
            await viewModel.ProjectTemplates.InitializeAsync();

            Assert.True(viewModel.ProjectTemplates.SavedPages.Count == 1);
            Assert.True(viewModel.ProjectTemplates.SavedPages.First().Count == 1);
            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 0);
            Assert.True(viewModel.Licenses.Count() == 1);
        }

        [Fact]
        public async Task ResolveDependenciesAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var testFrameworkName = "MVVMBasic";
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == testFrameworkName);
            await viewModel.ProjectTemplates.InitializeAsync();
            var settingsPage = FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Settings");
            UserSelectionService.AddTemplateAndDependencies(("Settings", settingsPage.Template), testFrameworkName);

            Assert.True(viewModel.ProjectTemplates.SavedPages.Count == 2, "Non expected result: two page groups");
            Assert.True(viewModel.ProjectTemplates.SavedPages.ElementAt(0).Count == 1, "Non expected result: One blank page");
            Assert.True(viewModel.ProjectTemplates.SavedPages.ElementAt(1).Count == 1, "Non expected result: One settings page");
            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 1, "Non expected result: One SettingStorage feature");
        }

        public async Task ResolveDependenciesAndLicensesAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var testFrameworkName = "MVVMBasic";
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == testFrameworkName);
            await viewModel.ProjectTemplates.InitializeAsync();
            var settingsPage = FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Settings");
            UserSelectionService.AddTemplateAndDependencies(("Settings", settingsPage.Template), testFrameworkName);
            Assert.True(viewModel.Licenses.Count == 2, "Non expected result: two licenses Microsoft.Toolkit.Uwp and Newtonsoft.Json");
        }

        [Fact]
        public async Task CanNotRemoveTemplateWithDependencyAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var testFrameworkName = "MVVMBasic";
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == testFrameworkName);
            await viewModel.ProjectTemplates.InitializeAsync();
            var settingsPage = FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Settings");
            UserSelectionService.AddTemplateAndDependencies(("Settings", settingsPage.Template), testFrameworkName);
            UserSelectionService.RemoveTemplate(viewModel.ProjectTemplates.SavedFeatures.First());

            // SettingsStorage can not be removed because Settings page depends on it
            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 1, "Settings page has been removed");
        }

        [Fact]
        public async Task RemoveHiddenFeaturesAsync()
        {
            // Default configuration: SplitView, CodeBehind, Blank page
            var testFrameworkName = "MVVMBasic";
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == testFrameworkName);
            await viewModel.ProjectTemplates.InitializeAsync();

            var gridPage = FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Grid");
            UserSelectionService.AddTemplateAndDependencies(("Grid", gridPage.Template), testFrameworkName);
            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 1, "Non expected result: Sample data filter");

            var chartPage = FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Chart");
            UserSelectionService.AddTemplateAndDependencies(("Chart", chartPage.Template), testFrameworkName);
            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 1, "Non expected result: Sample data filter");

            UserSelectionService.RemoveTemplate(viewModel.ProjectTemplates.SavedPages.First()[1]);
            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 1, "Non expected result: Sample data filter");

            UserSelectionService.RemoveTemplate(viewModel.ProjectTemplates.SavedPages.First()[1]);
            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 0, "No features expected");
        }

        [Fact]
        public async Task UpdateHomePageAsync()
        {
            // Configuration: SplitView, MVVM Basic, Blank page
            var testFrameworkName = "MVVMBasic";
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == "MVVMBasic");
            await viewModel.ProjectTemplates.InitializeAsync();
            var settingsPage = FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Settings");
            UserSelectionService.AddTemplateAndDependencies(("Page1", FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Blank").Template), testFrameworkName);
            UserSelectionService.AddTemplateAndDependencies(("Page2", FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Blank").Template), testFrameworkName);
            UserSelectionService.AddTemplateAndDependencies(("Page3", FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Blank").Template), testFrameworkName);

            // Drag Page1 in position 1 to Main in position 0
            OrderingService.SetDrag(viewModel.ProjectTemplates.SavedPages.First().ElementAt(1));
            OrderingService.SetDropTarget(viewModel.ProjectTemplates.SavedPages.First().ElementAt(0));
            OrderingService.SetDrop(viewModel.ProjectTemplates.SavedPages.First().ElementAt(0));

            // Check that Page1 is in position 0 and is the current Home Page
            Assert.True(viewModel.ProjectTemplates.HomeName == "Page1");
            Assert.True(viewModel.ProjectTemplates.SavedPages.First().ElementAt(0).ItemName == "Page1");
        }

        [Fact]
        public async Task RebuildLicensesAsync()
        {
            // Default configuration: SplitView, Code Behind, Blank page - 0 Licenses
            var testFrameworkName = "MVVMLight";
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            Assert.True(viewModel.Licenses.Count() == 1); // Microsoft.Toolkit.Uwp (CodeBehind)
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == testFrameworkName);
            Assert.True(viewModel.Licenses.Count() == 2); // Added MVVMLight lib
            await viewModel.ProjectTemplates.InitializeAsync();
            UserSelectionService.AddTemplateAndDependencies(("Settings", FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Settings").Template), testFrameworkName);
            Assert.True(viewModel.Licenses.Count() == 3); // Added Newtonsoft.Json
            UserSelectionService.AddTemplateAndDependencies(("HubNotifications", FindTemplate(viewModel.ProjectTemplates.FeatureGroups, "wts.Feat.HubNotifications").Template), testFrameworkName);
            Assert.True(viewModel.Licenses.Count() == 4); // Added WindowsAzure.Messaging.Managed
            UserSelectionService.RemoveTemplate(viewModel.ProjectTemplates.SavedFeatures.First(sf => sf.Identity == "wts.Feat.HubNotifications"));
            Assert.True(viewModel.Licenses.Count() == 3); // Deleted WindowsAzure.Messaging.Managed
        }

        private TemplateInfoViewModel FindTemplate(ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> groups, string identity)
        {
            foreach (var group in groups)
            {
                foreach (var template in group.Templates)
                {
                    if (template.Template.Identity == identity)
                    {
                        return template;
                    }
                }
            }
            return null;
        }
    }
}
