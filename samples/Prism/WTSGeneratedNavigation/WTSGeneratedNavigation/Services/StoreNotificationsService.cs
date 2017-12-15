using System;
using System.Threading.Tasks;

using Microsoft.Services.Store.Engagement;

namespace WTSGeneratedNavigation.Services
{
    internal class StoreNotificationsService : IStoreNotificationsService
    {
        public async Task InitializeAsync()
        {
            StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
            await engagementManager.RegisterNotificationChannelAsync();
        }
    }
}
