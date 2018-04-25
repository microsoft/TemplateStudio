using System;
using System.Threading.Tasks;

using AdvancedNavigationPaneProject.Services;

using Windows.ApplicationModel.Activation;

namespace AdvancedNavigationPaneProject.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        private readonly string _frameKey;
        private readonly Type _navElement;

        public DefaultLaunchActivationHandler(string frameKey, Type navElement)
        {
            _frameKey = frameKey;
            _navElement = navElement;
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            NavigationService.Navigate(_navElement, new NavigateConfig(_frameKey, args.Arguments));

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return !NavigationService.IsInitialized(_frameKey);
        }
    }
}
