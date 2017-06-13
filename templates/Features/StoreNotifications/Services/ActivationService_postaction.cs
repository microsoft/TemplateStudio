using System;
using Param_RootNamespace.Helpers;

namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
            await Singleton<StoreNotificationsFeatureService>.Instance.InitializeAsync();
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<StoreNotificationsFeatureService>.Instance;
//{--{
            yield break;//}--}
        }
    }
}