using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace RootNamespace.Services
{
    public static class LiveTileService
    {
        public static void EnableQueue()
        {
            var key = "NotificationQueueEnabled";
            var settings = ApplicationData.Current.LocalSettings.Values;            
            if (!settings.ContainsKey(key))
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                settings[key] = true;
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

        public static void SampleUpdate()
        {
            // See more information about Live Tiles Notifications
            //https://docs.microsoft.com/windows/uwp/controls-and-patterns/tiles-and-notifications-sending-a-local-tile-notification
            
            // These would be initialized with actual data
            string from = "Jennifer Parker";
            string subject = "Photos from our trip";
            string body = "Check out these awesome photos I took while in New Zealand!";

            // TODO: All values need to be XML escaped

            // Construct the tile content as a string
            string content = $@"
                <tile>
                    <visual>

                        <binding template='TileMedium'>
                            <text>{from}</text>
                            <text hint-style='captionSubtle'>{subject}</text>
                            <text hint-style='captionSubtle'>{body}</text>
                        </binding>

                        <binding template='TileWide'>
                            <text hint-style='subtitle'>{from}</text>
                            <text hint-style='captionSubtle'>{subject}</text>
                            <text hint-style='captionSubtle'>{body}</text>
                        </binding>

                    </visual>
                </tile>";

            // Load the string into an XmlDocument
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(content);

            // Then create the tile notification
            var notification = new TileNotification(doc);
            UpdateTile(notification);
        }

        public static async Task SamplePinSecondary(string pageName)
        {
            //TODO UWPTemplates: Call this method to Pin a Secondary Tile from a page.
            //You also must implement the navigation to this specific page in the OnLaunched event handler on App.xaml.cs
            SecondaryTile tile = new SecondaryTile(DateTime.Now.Ticks.ToString());
            tile.Arguments = pageName;
            tile.DisplayName = pageName;
            tile.VisualElements.Square44x44Logo = new Uri("ms-appx:///Assets/Square44x44Logo.scale-200.png");
            tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png");
            tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Wide310x150Logo.scale-200.png");
            tile.VisualElements.ShowNameOnSquare150x150Logo = true;
            tile.VisualElements.ShowNameOnWide310x150Logo = true;
            await PinSecondaryTileAsync(tile);
        }
    }
}