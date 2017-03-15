using RootNamespace.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace RootNamespace.Services
{
    public static partial class LiveTileService
    {
        private const string QueueEnabledKey = "NotificationQueueEnabled";

        public static void EnableQueue()
        {
            var queueEnabled = ApplicationData.Current.LocalSettings.Read<bool>(QueueEnabledKey);
            if (!queueEnabled)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                ApplicationData.Current.LocalSettings.Save(QueueEnabledKey, true);
            }
        }
        
        public static void UpdateTile(TileNotification notification)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        public static async Task<bool> PinSecondaryTileAsync(SecondaryTile tile, bool allowDuplicity = false)
        {
            if (!await IsAlreadyPinnedAsync(tile) || allowDuplicity)
            {
                return await tile.RequestCreateAsync();
            }
            return false;
        }

        private static async Task<bool> IsAlreadyPinnedAsync(SecondaryTile tile)
        {
            var secondaryTiles = await SecondaryTile.FindAllAsync();
            return secondaryTiles.Any(t => t.Arguments == tile.Arguments);
        }
    }
}