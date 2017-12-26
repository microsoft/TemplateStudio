// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.NewProject;

using Xunit;

namespace Microsoft.UI.Test
{
    [Collection("UI")]
    [Trait("ExecutionSet", "Minimum")]
    public class DataServiceTest : IClassFixture<TemplatesFixture>
    {
        private TemplatesFixture _fixture;

        public DataServiceTest(TemplatesFixture fixture)
        {
            _fixture = fixture;
            _fixture.InitializeFixture(ProgrammingLanguages.CSharp);
        }

        [Fact]
        public async Task LoadProjectSetupAsync()
        {
            var viewModel = new MainViewModel(GenContext.CurrentLanguage);
            await viewModel.ProjectSetup.InitializeAsync();
            Assert.True(viewModel.ProjectSetup.ProjectTypes.Count > 0);
            Assert.True(viewModel.ProjectSetup.Frameworks.Count > 0);
        }

        [Fact]
        public async Task LoadTemplatesAsync()
        {
            var viewModel = new MainViewModel(GenContext.CurrentLanguage);
            await viewModel.ProjectSetup.InitializeAsync();
            await viewModel.ProjectTemplates.InitializeAsync();

            Assert.True(viewModel.ProjectTemplates.PagesGroups.Count > 0);
            Assert.True(viewModel.ProjectTemplates.FeatureGroups.Count > 0);
        }
    }
}
