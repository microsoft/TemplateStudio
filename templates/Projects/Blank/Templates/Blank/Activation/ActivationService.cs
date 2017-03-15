using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uct.BlankProject.Activation
{
    class ActivationService
    {
        private readonly App _app;
        private readonly Type _defaultPage;

        public ActivationService(App app, Type defaultPage)
        {
            _app = app;
            _defaultPage = defaultPage;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            // Initialize things like registering background task before the app is loaded
            await InitializeAsync();

//#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                _app.DebugSettings.EnableFrameRateCounter = true;
            }
//#endif

            if (IsActivation(activationArgs))
            {
                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (Window.Current.Content == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    //TODO: MAYBE IS SHELLPAGE
                    Window.Current.Content = new Frame();
                } 
            }

            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsActivation(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultPage);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }

            // Tasks after activation
            await StartupAsync();
        }

        private async Task InitializeAsync()
        {
            await Task.FromResult(true).ConfigureAwait(false);
        }

        private async Task StartupAsync()
        {
            await Task.FromResult(true).ConfigureAwait(false);
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {

            yield break;
        }

        private bool IsActivation(object args)
        {
            return args is IActivatedEventArgs;
        }
    }
}
