using System;
using System.Threading.Tasks;

using Microsoft.Services.Store.Engagement;

namespace Param_RootNamespace.Services
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
