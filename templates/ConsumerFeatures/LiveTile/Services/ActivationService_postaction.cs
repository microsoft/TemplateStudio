using System;
using RootNamespace.Helpers;

namespace ItemNamespace.Services
{
    internal class ActivationService
    {
        private async Task InitializeAsync()
        {
            await Singleton<LiveTileService>.Instance.EnableQueueAsync();
        }
        private async Task StartupAsync()
        {
            Singleton<LiveTileService>.Instance.SampleUpdate();
        }
        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<LiveTileService>.Instance;
        }
    }
}