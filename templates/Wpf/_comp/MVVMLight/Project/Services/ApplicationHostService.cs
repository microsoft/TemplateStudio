using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Services
{
    public class ApplicationHostService : IApplicationHostService
    {
        private readonly INavigationService _navigationService;
        private IShellWindow _shellWindow;

        public ApplicationHostService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task StartAsync()
        {
            // Initialize services that you need before app activation
            await InitializeAsync();

            _shellWindow = SimpleIoc.Default.GetInstance<IShellWindow>();
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _shellWindow.ShowWindow();
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
