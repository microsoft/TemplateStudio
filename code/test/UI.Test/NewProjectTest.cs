// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.Templates.UI.ViewModels.Common;

using Xunit;

namespace UI.Test
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
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == "MVVMBasic");
            await viewModel.ProjectTemplates.InitializeAsync();
            var settingsPage = FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Settings");
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("Settings", settingsPage.Template));

            Assert.True(viewModel.ProjectTemplates.SavedPages.Count == 2);
            Assert.True(viewModel.ProjectTemplates.SavedPages.ElementAt(0).Count == 1);
            Assert.True(viewModel.ProjectTemplates.SavedPages.ElementAt(1).Count == 1);

            Assert.True(viewModel.ProjectTemplates.SavedFeatures.Count == 1); // SettingsStorage feature added bt SettingsPage
            Assert.True(viewModel.Licenses.Count == 2); // Newtonsoft.Json added by SettingsStorage and Microsoft.Toolkit.Uwp added by MVVM Basic
        }

        [Fact]
        public async Task UpdateHomePageAsync()
        {
            // Configuration: SplitView, MVVM Basic, Blank page
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == "MVVMBasic");
            await viewModel.ProjectTemplates.InitializeAsync();
            var settingsPage = FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Settings");
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("Page1", FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Blank").Template));
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("Page2", FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Blank").Template));
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("Page3", FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Blank").Template));

            // Drag Page1 in position 1 to Main in position 0
            viewModel.Ordering.SetDrag(viewModel.ProjectTemplates.SavedPages.First().ElementAt(1));
            viewModel.Ordering.SetDropTarget(viewModel.ProjectTemplates.SavedPages.First().ElementAt(0));
            viewModel.Ordering.SetDrop(viewModel.ProjectTemplates.SavedPages.First().ElementAt(0));

            // Check that Page1 is in position 0 and is the current Home Page
            Assert.True(viewModel.ProjectTemplates.HomeName == "Page1");
            Assert.True(viewModel.ProjectTemplates.SavedPages.First().ElementAt(0).ItemName == "Page1");
        }

        [Fact]
        public async Task RebuildLicensesAsync()
        {
            // Default configuration: SplitView, Code Behind, Blank page - 0 Licenses
            var viewModel = new MainViewModel();
            await viewModel.ProjectSetup.InitializeAsync();
            Assert.True(viewModel.Licenses.Count() == 1); // Microsoft.Toolkit.Uwp (CodeBehind)
            viewModel.ProjectSetup.SelectedFramework = viewModel.ProjectSetup.Frameworks.First(pt => pt.Name == "MVVMLight");
            Assert.True(viewModel.Licenses.Count() == 2); // Added MVVMLight lib
            await viewModel.ProjectTemplates.InitializeAsync();
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("Settings", FindTemplate(viewModel.ProjectTemplates.PagesGroups, "wts.Page.Settings.MVVMLight").Template));
            Assert.True(viewModel.Licenses.Count() == 3); // Added Newtonsoft.Json
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("HubNotifications", FindTemplate(viewModel.ProjectTemplates.FeatureGroups, "wts.Feat.HubNotifications").Template));
            Assert.True(viewModel.Licenses.Count() == 4); // Added WindowsAzure.Messaging.Managed
            viewModel.ProjectTemplates.RemoveTemplate(viewModel.ProjectTemplates.SavedFeatures.First(sf => sf.Identity == "wts.Feat.HubNotifications"), false);
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
