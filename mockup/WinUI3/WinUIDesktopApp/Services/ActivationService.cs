using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;

using WinUIDesktopApp.Activation;
using WinUIDesktopApp.Contracts.Services;
using WinUIDesktopApp.Contracts.Views;

namespace WinUIDesktopApp.Services
{
    public class ActivationService : IActivationService
    {
        private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
        private readonly IEnumerable<IActivationHandler> _activationHandlers;
        private readonly INavigationService _navigationService;
        private readonly IThemeSelectorService _themeSelectorService;

        public ActivationService(IShellWindow shellWindow, ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, INavigationService navigationService, IThemeSelectorService themeSelectorService)
        {
            App.MainWindow = shellWindow as Window;
            _defaultHandler = defaultHandler;
            _activationHandlers = activationHandlers;
            _navigationService = navigationService;
            _themeSelectorService = themeSelectorService;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            // Initialize services that you need before app activation
            // take into account that the splash screen is shown while this code runs.
            await InitializeAsync();

            App.MainWindow.Activate();

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            await HandleActivationAsync(activationArgs);

            // Tasks after activation
            await StartupAsync();
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = _activationHandlers
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (_defaultHandler.CanHandle(activationArgs))
            {
                await _defaultHandler.HandleAsync(activationArgs);
            }
        }

        private async Task InitializeAsync()
        {
            await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
            await Task.CompletedTask;
        }

        private async Task StartupAsync()
        {
            await _themeSelectorService.SetRequestedThemeAsync();
            await Task.CompletedTask;
        }
    }
}
