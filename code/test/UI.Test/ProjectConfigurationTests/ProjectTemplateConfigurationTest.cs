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
    public class ProjectTemplateConfigurationTest : IClassFixture<UiTemplatesFixture>
    {
        // Project Types
        private const string TestProjectType = "TestProjectType";
        private const string TestSecondProjectType = "TestSecondProjectType";

        // Frameworks
        private const string TestFramework = "TestFramework";

        // Pages
        private const string TestPage = "Microsoft.Templates.Test.TestPageTemplate.CSharp";
        private const string TestSecondPage = "Microsoft.Templates.Test.TestSecondPageTemplate.CSharp";

        // Features
        private const string TestFeature = "Microsoft.Templates.Test.TestFeatureTemplate.CSharp";

        // Services
        private const string ServiceWebApi = "wts.Service.WebApi";

        // Platforms
        private const string TestPlatform = "test";

        public ProjectTemplateConfigurationTest(UiTemplatesFixture fixture)
        {
            fixture.InitializeFixture("test", ProgrammingLanguages.CSharp);
        }

        [Fact]
        public async Task ProjectInitUpdatedConfigurationAsync()
        {
            // Default configuration: SplitView, MvvmToolkit, Blank page
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

        //[Fact]
        //public async Task ResolveDependenciesAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);
        //    await viewModel.OnTemplatesAvailableAsync();

        //    var settingsTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageSettings);
        //    var numOfDependencies = settingsTemplate.Dependencies?.Count();
        //    await AddTemplateAsync(viewModel, settingsTemplate);
        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.Equal(2, userSelection.Pages.Count);
        //    Assert.Equal(numOfDependencies, userSelection.Features.Count);
        //    Assert.Equal(FeatureSettingsStorage, userSelection.Features.First().TemplateId);
        //}

        //[Fact]
        //public async Task ResolveDependenciesAndLicensesAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);
        //    await viewModel.OnTemplatesAvailableAsync();

        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.Single(userSelection.Pages);
        //    Assert.Empty(userSelection.Features);
        //    Assert.Equal(2, viewModel.UserSelection.Licenses.Count);
        //    var settingsTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageSettings);
        //    var numOfDependencies = settingsTemplate.Dependencies?.Count();
        //    await AddTemplateAsync(viewModel, settingsTemplate);
        //    userSelection = viewModel.UserSelection.GetUserSelection();
        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.Equal(2, userSelection.Pages.Count);
        //    Assert.Equal(numOfDependencies, userSelection.Features.Count);
        //    Assert.Equal(3, viewModel.UserSelection.Licenses.Count);
        //}

        //[Fact]
        //public async Task RemovePageAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);
        //    await viewModel.OnTemplatesAvailableAsync();

        //    await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.True(userSelection.Pages.Count == 2);
        //    DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 1);
        //    userSelection = viewModel.UserSelection.GetUserSelection();

        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.Single(userSelection.Pages);
        //}

        //[Fact]
        //public async Task RemoveTemplateWithHiddenDependencyAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);
        //    await viewModel.OnTemplatesAvailableAsync();

        //    await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Service].Groups, ServiceWebApi));
        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.Equal(3, userSelection.Services.Count);
        //    DeleteTemplate(TemplateType.Service, viewModel.UserSelection, 2);
        //    userSelection = viewModel.UserSelection.GetUserSelection();
        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.True(userSelection.Services.Count == 1);
        //}

        //[Fact]
        //public async Task CanNotRemoveHomePageAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);

        //    await viewModel.OnTemplatesAvailableAsync();

        //    DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 0);
        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.Single(userSelection.Pages);
        //}

        //[Fact]
        //public async Task CanNotRemoveTemplateWithDependencyAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    viewModel.UserSelection.ResetUserSelection();
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);
        //    await viewModel.OnTemplatesAvailableAsync();

        //    var settingsTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageSettings);
        //    var numOfDependencies = settingsTemplate.Dependencies?.Count();
        //    await AddTemplateAsync(viewModel, settingsTemplate);
        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.Equal(numOfDependencies, userSelection.Features.Count);
        //    DeleteTemplate(TemplateType.Feature, viewModel.UserSelection, 0);
        //    userSelection = viewModel.UserSelection.GetUserSelection();
        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.Equal(numOfDependencies, userSelection.Features.Count);
        //}

        //[Fact]
        //public async Task RemoveHiddenFeaturesAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);
        //    await viewModel.OnTemplatesAvailableAsync();

        //    var chartTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageChart);
        //    var gridTemplate = GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageGrid);
        //    var numOfDependencies = chartTemplate.Dependencies?.Count();
        //    await AddTemplateAsync(viewModel, chartTemplate);
        //    await AddTemplateAsync(viewModel, gridTemplate);

        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.Equal(3, userSelection.Pages.Count);
        //    Assert.Equal(numOfDependencies, userSelection.Services.Count);

        //    DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 2);
        //    userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.Equal(2, userSelection.Pages.Count);
        //    Assert.Equal(numOfDependencies, userSelection.Services.Count);

        //    DeleteTemplate(TemplateType.Page, viewModel.UserSelection, 1);
        //    userSelection = viewModel.UserSelection.GetUserSelection();
        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.Single(userSelection.Pages);
        //    Assert.Equal(numOfDependencies, userSelection.Services.Count + 1);
        //}

        //[Fact]
        //public async Task ReorderPagesUsingKeyboardAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);
        //    await viewModel.OnTemplatesAvailableAsync();

        //    await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
        //    await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
        //    await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
        //    await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.True(userSelection.Pages[1].Name == "Blank");
        //    Assert.True(userSelection.Pages[2].Name == "Blank1");
        //    Assert.True(userSelection.Pages[3].Name == "Blank2");
        //    Assert.True(userSelection.Pages[4].Name == "Blank3");
        //    var pages = viewModel.UserSelection.Groups.First(g => g.TemplateType == TemplateType.Page);
        //    pages.EnableOrdering(null);
        //    pages.SelectedItem = pages.Items[2]; // Select Blank1
        //    pages.MoveDownCommand.Execute(null);
        //    userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.True(userSelection.Pages[1].Name == "Blank");
        //    Assert.True(userSelection.Pages[2].Name == "Blank2");
        //    Assert.True(userSelection.Pages[3].Name == "Blank1");
        //    Assert.True(userSelection.Pages[4].Name == "Blank3");
        //    pages.MoveUpCommand.Execute(null);
        //    userSelection = viewModel.UserSelection.GetUserSelection();
        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.True(userSelection.Pages[1].Name == "Blank");
        //    Assert.True(userSelection.Pages[2].Name == "Blank1");
        //    Assert.True(userSelection.Pages[3].Name == "Blank2");
        //    Assert.True(userSelection.Pages[4].Name == "Blank3");
        //}

        //[Fact]
        //public async Task UpdateHomePageAsync()
        //{
        //    // Default configuration: SplitView, MvvmToolkit, Blank page
        //    var stylesProviders = new UITestStyleValuesProvider();
        //    var viewModel = new MainViewModel(null, stylesProviders);
        //    var context = new UserSelectionContext(GenContext.CurrentLanguage, Platforms.Uwp);
        //    viewModel.Initialize(context);
        //    await viewModel.OnTemplatesAvailableAsync();

        //    await AddTemplateAsync(viewModel, GetTemplate(viewModel.StepsViewModels[TemplateType.Page].Groups, PageBlank));
        //    var userSelection = viewModel.UserSelection.GetUserSelection();
        //    Assert.True(userSelection.Pages[0].Name == "Main");
        //    Assert.True(userSelection.Pages[1].Name == "Blank");
        //    Assert.True(userSelection.HomeName == "Main");
        //    var pages = viewModel.UserSelection.Groups.First(g => g.TemplateType == TemplateType.Page);
        //    pages.EnableOrdering(null);
        //    pages.SelectedItem = pages.Items[1]; // Select Blank
        //    pages.MoveUpCommand.Execute(null);
        //    userSelection = viewModel.UserSelection.GetUserSelection();
        //    viewModel.UnsubscribeEventHandlers();

        //    Assert.True(userSelection.Pages[0].Name == "Blank");
        //    Assert.True(userSelection.Pages[1].Name == "Main");
        //    Assert.True(userSelection.HomeName == "Blank");
        //}

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
