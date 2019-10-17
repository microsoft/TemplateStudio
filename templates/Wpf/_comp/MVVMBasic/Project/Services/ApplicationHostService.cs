using System.Threading;
using System.Threading.Tasks;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.ViewModels;
using Microsoft.Extensions.Hosting;

namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IHostedService
    {
        private readonly INavigationService _navigationService;
        private readonly IShellWindow _shellWindow;

        public ApplicationHostService(INavigationService navigationService, IShellWindow shellWindow)
        {
            _navigationService = navigationService;
            _shellWindow = shellWindow;
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(Param_HomeNameViewModel).FullName);

            // Tasks after activation
            await StartupAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
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
