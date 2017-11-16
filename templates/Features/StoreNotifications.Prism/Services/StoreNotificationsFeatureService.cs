using System;
using System.Threading.Tasks;

using Microsoft.Services.Store.Engagement;

namespace Param_RootNamespace.Services
{
    internal class StoreNotificationsFeatureService : IStoreNotificationsFeatureService
    {
        public async Task InitializeAsync()
        {
            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
        }
    }
}
