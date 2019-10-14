using System.Threading.Tasks;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly INavigationService _navigationService;
        private readonly IShellPage _shellPage;

        public ApplicationHostService(INavigationService navigationService, IShellPage shellPage)
        {
            _navigationService = navigationService;
            _shellPage = shellPage;
            _navigationService.Initialize(_shellPage.GetNavigationFrame());
        }

        public async Task StartAsync()
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            _shellPage.ShowWindow();
            _navigationService.NavigateTo(typeof(Param_HomeNameViewModel).FullName);

            // Tasks after activation
            await StartupAsync();
        }

        public async Task StopAsync()
        {
            await Task.CompletedTask;
        }

        private async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            await Task.CompletedTask;
        }
    }
}
