using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DotNetCoreWpfApp.Contracts.Services;
using DotNetCoreWpfApp.Contracts.Views;
using DotNetCoreWpfApp.ViewModels;

using Microsoft.Extensions.Hosting;

namespace DotNetCoreWpfApp.Services
{
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly IPersistAndRestoreService _persistAndRestoreService;
        private readonly IThemeSelectorService _themeSelectorService;
        private IShellWindow _shellWindow;

        public ApplicationHostService(IServiceProvider serviceProvider, INavigationService navigationService, IThemeSelectorService themeSelectorService, IPersistAndRestoreService persistAndRestoreService)
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _themeSelectorService = themeSelectorService;
            _persistAndRestoreService = persistAndRestoreService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            await HandleActivationAsync();

            // Tasks after activation
            await StartupAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _persistAndRestoreService.PersistData();
            await Task.CompletedTask;
        }

        private async Task InitializeAsync()
        {
            _persistAndRestoreService.RestoreData();
            _themeSelectorService.SetTheme();
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            await Task.CompletedTask;
        }

        private async Task HandleActivationAsync()
        {
            if (App.Current.Windows.OfType<IShellWindow>().Count() == 0)
            {
                // Default activation that navigates to the apps default page
                _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
                _navigationService.Initialize(_shellWindow.GetNavigationFrame());
                _shellWindow.ShowWindow();
                _navigationService.NavigateTo(typeof(MainViewModel).FullName);
                await Task.CompletedTask;
            }
        }
    }
}
