using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.ViewModels.NewProject;
using Xunit;

namespace Microsoft.UI.Test.ProjectConfigurationTests
{
    [Collection("UI")]
    [Trait("ExecutionSet", "Minimum")]
    public class ProjectTemplateConfigurationTest : IClassFixture<UITemplatesFixture>
    {
        // Project Types
        private const string TestProjectType = "TestProjectType";
        private const string TestSecondProjectType = "TestSecondProjectType";

        // Frameworks
        private const string TestFramework = "TestFramework";

        // Pages
        private const string TestPage = "Microsoft.Templates.Test.TestPageTemplate.CSharp";
        private const string TestSecondPage = "Microsoft.Templates.Test.TestSecondPageTemplate.CSharp";
        private const string TestPageWithDependencies = "Microsoft.Templates.Test.TestPageWithDependenciesTemplate.CSharp";

        // Features
        private const string TestFeature = "Microsoft.Templates.Test.TestFeatureTemplate.CSharp";

        // Services
        private const string TestService = "Microsoft.Templates.Test.ServiceTemplate.CSharp";

        // Platforms
        private const string TestPlatform = "test";

        public ProjectTemplateConfigurationTest(UITemplatesFixture fixture)
        {
            fixture.InitializeFixture(TestPlatform, ProgrammingLanguages.CSharp);
        }

        [Fact]
        public async Task ProjectInitUpdatedConfigurationAsync()
        {
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, TestPlatform);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();

            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Equal(TestProjectType, userSelection.Context.ProjectType);
            Assert.Equal(TestFramework, userSelection.Context.FrontEndFramework);
            Assert.Equal(TestPage, userSelection.Pages.First().TemplateId);
            await SetProjectTypeAsync(viewModel, TestSecondProjectType);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Equal(TestSecondProjectType, userSelection.Context.ProjectType);
            Assert.Equal(TestSecondPage, userSelection.Pages.First().TemplateId);
        }

        [Fact]
        public async Task ResolveDependenciesAsync()
        {
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, TestPlatform);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();

            var pageWithDependenciesTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, TestPageWithDependencies);
            var numOfDependencies = pageWithDependenciesTemplate.Dependencies?.Count();
            await AddTemplateAsync(viewModel, pageWithDependenciesTemplate);
            var userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Equal(2, userSelection.Pages.Count);
            Assert.Equal(numOfDependencies, userSelection.Features.Count);
            Assert.Equal(TestFeature, userSelection.Features.First().TemplateId);
        }

        [Fact]
        public async Task ResolveDependenciesAndLicensesAsync()
        {
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, TestPlatform);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();

            var userSelection = viewModel.UserSelection.GetUserSelection();
            Assert.Single(userSelection.Pages);
            Assert.Empty(userSelection.Features);
            Assert.Equal(2, viewModel.UserSelection.Licenses.Count);
            var pageWithDependenciesTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, TestPageWithDependencies);
            var numOfDependencies = pageWithDependenciesTemplate.Dependencies?.Count();
            await AddTemplateAsync(viewModel, pageWithDependenciesTemplate);
            userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Equal(2, userSelection.Pages.Count);
            Assert.Equal(numOfDependencies, userSelection.Features.Count);
            Assert.Equal(3, viewModel.UserSelection.Licenses.Count);
        }

        [Fact]
        public async Task RemovePageAsync()
        {
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, TestPlatform);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();

            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, TestPage));
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
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, TestPlatform);
            viewModel.Initialize(context);
            await viewModel.OnTemplatesAvailableAsync();

            await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Service].Groups, TestService));
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
            var stylesProviders = new UITestStyleValuesProvider();
            var viewModel = new MainViewModel(null, stylesProviders);
            var context = new UserSelectionContext(GenContext.CurrentLanguage, TestPlatform);
            viewModel.Initialize(context);

            await viewModel.OnTemplatesAvailableAsync();

            DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 0);
            var userSelection = viewModel.UserSelection.GetUserSelection();
            viewModel.UnsubscribeEventHandlers();

            Assert.Single(userSelection.Pages);
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
