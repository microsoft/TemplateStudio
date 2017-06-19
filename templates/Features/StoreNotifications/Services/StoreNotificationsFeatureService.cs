using Microsoft.Services.Store.Engagement;

using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;

using Param_RootNamespace.Activation;

namespace Param_RootNamespace.Services
{
    internal class StoreNotificationsFeatureService : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        public async Task InitializeAsync()
        {
            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            string originalArgs = engagementManager.ParseArgumentsAndTrackAppLaunch(toastActivationArgs.Argument);

            // Use the originalArgs variable to access the original arguments
            // that were passed to the app.

            await Task.CompletedTask;
        }
    }
}
