// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Xunit;

namespace Microsoft.UI.Test.ProjectTests
{
    [Collection("UI")]
    [Trait("Group", "Minimum")]
    public class NewWinUICsProjectTest : IClassFixture<WinUICsPlatformTemplatesFixture>
    {
        private const string DefaultProjectType = "Blank";
        private const string DefaultFramework = "None";

        public NewWinUICsProjectTest(WinUICsPlatformTemplatesFixture fixture)
        {
            fixture.InitializeFixture(Platforms.WinUI, ProgrammingLanguages.CSharp);
        }

        [Fact]
        public async Task WinUiProjectLoadStepsAsync()
        {
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.WinUI);
            context.AddAppModel(AppModels.Desktop);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();
            viewModel.UnsubscribeEventHandlers();

            Assert.True(viewModel.ProjectType.Items.Any());
            Assert.True(viewModel.Framework.Items.Any());
            Assert.True(viewModel.StepsViewModels[TemplateType.Feature].Groups.Any());
        }

        [Fact]
        public async Task WinUiProjectLoadDefaultStepsAsync()
        {
            // Default configuration: Blank, None
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.WinUI);
            context.AddAppModel(AppModels.Desktop);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();
            viewModel.UnsubscribeEventHandlers();

            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Equal(DefaultProjectType, userSelection.Context.ProjectType);
            Assert.Equal(DefaultFramework, userSelection.Context.FrontEndFramework);
            Assert.Empty(userSelection.Pages);
            Assert.Empty(userSelection.Services);
            Assert.Empty(userSelection.Testing);
        }
    }
}
