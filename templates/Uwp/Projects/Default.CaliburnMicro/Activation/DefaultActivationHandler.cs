using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Windows.ApplicationModel.Activation;

namespace Param_RootNamespace.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly Type _navElement;
        private readonly INavigationService _navigationService;

        public DefaultActivationHandler(Type navElement, INavigationService navigationService)
        {
            _navElement = navElement;
            _navigationService = navigationService;
        }

        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
        {
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            object arguments = null;
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                arguments = launchArgs.Arguments;
            }

            _navigationService.NavigateToViewModel(_navElement, arguments);
            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(IActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return _navigationService.SourcePageType == null && _navElement != null;
        }
    }
}
