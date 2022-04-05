using System;
using System.Threading.Tasks;

using Microsoft.Services.Store.Engagement;

namespace Param_RootNamespace.Services
{
    internal class StoreNotificationsFeatureService : IStoreNotificationsFeatureService
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
    }
}
