using System;
using RootNamespace.Helper;

namespace ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task StartupAsync()
        {
            Singleton<ToastNotificationsService>.Instance.ShowToastNotificationSample();
        }
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<ToastNotificationsService>.Instance;
        }
    }
}