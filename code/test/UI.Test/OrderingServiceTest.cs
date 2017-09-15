// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.NewProject;

using Xunit;

namespace UI.Test
{
    public class OrderingServiceTest : IClassFixture<TemplatesFixture>
    {
        private TemplatesFixture _fixture;

        public OrderingServiceTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture("C#");
        }

        [Fact]
        public void DragAndDrop()
        {
            var viewModel = new MainViewModel();
            viewModel.ProjectTemplates.ContextFramework = new MetadataInfoViewModel(GenContext.ToolBox.Repo.GetFrameworks().First());
            viewModel.ProjectTemplates.ContextProjectType = new MetadataInfoViewModel(GenContext.ToolBox.Repo.GetProjectTypes().First());
            var mainPage = _fixture.Repository.Get(t => t.Identity == "wts.Page.Blank").FirstOrDefault();
            var chartPage = _fixture.Repository.Get(t => t.Identity == "wts.Page.Chart").FirstOrDefault();
            var mapPage = _fixture.Repository.Get(t => t.Identity == "wts.Page.Map").FirstOrDefault();
            // Page at position 0
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("Main", mainPage), false);
            // Page at position 1
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("Chart", chartPage), false);
            // Page at position 2
            viewModel.ProjectTemplates.AddTemplateAndDependencies(("Map", mapPage), false);

            // Drag Chart page and drop on Mar page
            viewModel.Ordering.SetDrag(viewModel.ProjectTemplates.SavedPages.First().ElementAt(1));
            viewModel.Ordering.SetDropTarget(viewModel.ProjectTemplates.SavedPages.First().ElementAt(2));
            viewModel.Ordering.SetDrop(viewModel.ProjectTemplates.SavedPages.First().ElementAt(2));

            // Check that now, Map page is in position 1 and Chart page is in position 2
            Assert.True(viewModel.ProjectTemplates.SavedPages.First().ElementAt(1).ItemName == "Map");
            Assert.True(viewModel.ProjectTemplates.SavedPages.First().ElementAt(2).ItemName == "Chart");
        }
    }
}
