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
            try
            {
                var engagementManager = StoreServicesEngagementManager.GetDefault();
                await engagementManager.RegisterNotificationChannelAsync();
            }
            catch (Exception)
            {
                // TODO: Channel registration call can fail, please handle exceptions as appropriate to your scenario.
            }
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

            var engagementManager = StoreServicesEngagementManager.GetDefault();
            string originalArgs = engagementManager.ParseArgumentsAndTrackAppLaunch(toastActivationArgs.Argument);

            //// Use the originalArgs variable to access the original arguments passed to the app.

            await Task.CompletedTask;
        }
    }
}
