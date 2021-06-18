// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Xunit;

namespace Microsoft.UI.Test.ProjectTests
{
    [Collection("UI")]
    [Trait("ExecutionSet", "Minimum")]
    public class NewUwpProjectTest : IClassFixture<PlatformTemplatesFixture>
    {
        private const string DefaultProjectType = "SplitView";
        private const string DefaultFramework = "MVVMToolkit";
        private const string DefaultPage = "wts.Page.Blank";

        public NewUwpProjectTest(PlatformTemplatesFixture fixture)
        {
            fixture.InitializeFixture(Platforms.Uwp, ProgrammingLanguages.CSharp);
        }

        [Fact]
        public async Task UwpProjectLoadStepsAsync()
        {
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();
            viewModel.UnsubscribeEventHandlers();

            Assert.True(viewModel.ProjectType.Items.Any());
            Assert.True(viewModel.Framework.Items.Any());
            Assert.True(viewModel.StepsViewModels[TemplateType.Page].Groups.Any());
            Assert.True(viewModel.StepsViewModels[TemplateType.Feature].Groups.Any());
            Assert.True(viewModel.StepsViewModels[TemplateType.Service].Groups.Any());
            Assert.True(viewModel.StepsViewModels[TemplateType.Testing].Groups.Any());
        }

        [Fact]
        public async Task UwpProjectLoadDefaultStepsAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();
            viewModel.UnsubscribeEventHandlers();

            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Equal(DefaultProjectType, userSelection.Context.ProjectType);
            Assert.Equal(DefaultFramework, userSelection.Context.FrontEndFramework);
            Assert.Equal(DefaultPage, userSelection.Pages.First().TemplateId);
            Assert.Empty(userSelection.Features);
            Assert.Empty(userSelection.Services);
            Assert.Empty(userSelection.Testing);
        }
    }
}
