// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using Xunit;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Microsoft.Templates.Core.Gen;

namespace UI.Test
{
    public class DataServiceTest : IClassFixture<TemplatesFixture>
    {
        private TemplatesFixture _fixture;

        public DataServiceTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture("C#");
        }

        [Fact]
        public void LoadProjectTypes()
        {
            var viewModel = new MainViewModel();
            ObservableCollection<MetadataInfoViewModel> projectTypes = new ObservableCollection<MetadataInfoViewModel>();
            Assert.True(DataService.LoadProjectTypes(projectTypes));
        }

        [Fact]
        public void LoadFrameworks()
        {
            var viewModel = new MainViewModel();
            ObservableCollection<MetadataInfoViewModel> frameworks = new ObservableCollection<MetadataInfoViewModel>();
            Assert.True(DataService.LoadFrameworks(frameworks, "Blank"));
        }

        [Fact]
        public void LoadTemplates()
        {
            var viewModel = new MainViewModel();
            viewModel.ProjectSetup.SelectedFramework = new MetadataInfoViewModel(GenContext.ToolBox.Repo.GetFrameworks().FirstOrDefault(f => f.Name == "MVVMBasic"));
            ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> pagesGroups = new ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>>();
            ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>> featuresGroups = new ObservableCollection<ItemsGroupViewModel<TemplateInfoViewModel>>();

            var totalPages = DataService.LoadTemplatesGroups(pagesGroups, TemplateType.Page, "MVVMBasic");
            var totalFeatures = DataService.LoadTemplatesGroups(featuresGroups, TemplateType.Feature, "MVVMBasic");

            Assert.True(totalPages > 0);
            Assert.True(totalFeatures > 0);
        }
    }
}
