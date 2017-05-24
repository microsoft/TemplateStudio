using System;
using Param_RootNamespace.Helpers;

namespace Param_ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
            await Singleton<LiveTileFeatureService>.Instance.EnableQueueAsync();
        }

        private async Task StartupAsync()
        {
            Singleton<LiveTileFeatureService>.Instance.SampleUpdate();
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<LiveTileFeatureService>.Instance;
//{--{

            yield break;//}--}
        }
    }
}