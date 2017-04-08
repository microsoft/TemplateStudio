using System;
using RootNamespace.Helper;

namespace ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
            Singleton<BackgroundTaskService>.Instance.RegisterBackgroundTasks();
        }
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<BackgroundTaskService>.Instance;
        }
    }
}