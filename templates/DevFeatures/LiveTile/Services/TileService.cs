using System;
using Windows.UI.Notifications;
using Windows.Storage;

namespace RootNamespace.Services
{
    public class TileService
    {
        private static volatile TileService instance;
        private static object syncRoot = new Object();

        public static TileService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) instance = new TileService();
                    }
                }

                return instance;
            }
        }

        private TileService() { }

        public void EnableQueue()
        {
            var key = "NotificationQueueEnabled";
            var settings = ApplicationData.Current.LocalSettings.Values;            
            if (!settings.ContainsKey(key))
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                settings[key] = true;
            }            
        }

        public void UpdateTile()
        {
            var wideTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150ImageAndText01);

            var text = $"Hello World! This is {DateTime.Now}.";
            wideTileXml.GetElementsByTagName("text")[0].InnerText = text;

            var wideTileNotification = new TileNotification(wideTileXml);

            int tileCount = GetTileCount();
            wideTileNotification.Tag = $"{tileCount}";

            TileUpdateManager.CreateTileUpdaterForApplication().Update(wideTileNotification);
        }

        private int GetTileCount()
        {
            var key = "NotificationsCount";
            var settings = ApplicationData.Current.LocalSettings.Values;

            int value = (settings.ContainsKey(key)) ? (int)settings[key] : 1;
            settings[key] = value + 1;
            return value;
        }
    }
}