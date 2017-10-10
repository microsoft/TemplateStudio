using System;
using System.Linq;
using System.Threading.Tasks;

using LiveTileActivationSample.MVVMBasic.Activation;
using LiveTileActivationSample.MVVMBasic.Helpers;
using LiveTileActivationSample.MVVMBasic.Views;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace LiveTileActivationSample.MVVMBasic.Services
{
    internal partial class LiveTileService : ActivationHandler<LaunchActivatedEventArgs>
    {
        private const string QueueEnabledKey = "NotificationQueueEnabled";
        public const string SecondarySectionPageID = "SecondarySectionPage";

        public async Task EnableQueueAsync()
        {
            var queueEnabled = await ApplicationData.Current.LocalSettings.ReadAsync<bool>(QueueEnabledKey);
            if (!queueEnabled)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                await ApplicationData.Current.LocalSettings.SaveAsync(QueueEnabledKey, true);
            }
        }

        public void UpdateTile(TileNotification notification)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        public async Task<bool> PinSecondaryTileAsync(SecondaryTile tile, bool allowDuplicity = false)
        {
            if (!await IsAlreadyPinnedAsync(tile) || allowDuplicity)
            {
                return await tile.RequestCreateAsync();
            }

            return false;
        }

        private async Task<bool> IsAlreadyPinnedAsync(SecondaryTile tile)
        {
            var secondaryTiles = await SecondaryTile.FindAllAsync();
            return secondaryTiles.Any(t => t.Arguments == tile.Arguments);
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            if (LaunchFromSecondaryTile(args))
            {
                NavigationService.Navigate<SecondarySectionPage>();
            }
            else if (LaunchFromLiveTileUpdate(args))
            {
                NavigationService.Navigate<LiveTileUpdatePage>();
            }
            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            return LaunchFromSecondaryTile(args) || LaunchFromLiveTileUpdate(args);
        }

        private bool LaunchFromSecondaryTile(LaunchActivatedEventArgs args)
        {
            return args.Arguments == SecondarySectionPageID;
        }

        private bool LaunchFromLiveTileUpdate(LaunchActivatedEventArgs args)
        {
            if (args.TileActivatedInfo != null)
            {
                var tileUpdatesArguments = args.TileActivatedInfo.RecentlyShownNotifications;
                if (tileUpdatesArguments.Count > 0)
                {
                    var allArguments = tileUpdatesArguments.Select(i => i.Arguments).ToArray();
                    return allArguments.Any(a => a == "Jennifer Parker");
                }
            }
            return false;
        }
    }
}
