using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

using wts.DefaultProject.Services;

namespace wts.DefaultProject.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            NavigationService.Navigate(_navElement, args.Arguments);

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null;
        }
    }
}
