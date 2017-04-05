using System;
using RootNamespace.Helper;

namespace ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task StartupAsync()
        {
            //Singleton<HubNotificationsService>.Instance.InitializeAsync();
        }
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<HubNotificationsService>.Instance;
        }
    }
}