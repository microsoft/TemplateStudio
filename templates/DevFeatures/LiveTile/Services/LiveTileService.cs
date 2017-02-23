using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

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
    }
}