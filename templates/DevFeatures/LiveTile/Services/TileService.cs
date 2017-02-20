using System;
using Windows.UI.Notifications;
using Windows.Storage;

namespace RootNamespace.Services
{
    public class TileService
    {
        private static Lazy<TileService> instanceHolder = new Lazy<TileService>(() => new TileService());

        private static TileService Instance { get => instanceHolder.Value; }

        private TileService() { }

        public static void EnableQueue()
        {
            lock (Instance)
            {
                Instance.DoEnableQueue();
            }
        }
        
        public void UpdateTile()
        {
            lock (Instance)
            {
                Instance.DoUpdateTile();
            }
        }

        private void DoEnableQueue()
        {
            var key = "NotificationQueueEnabled";
            var settings = ApplicationData.Current.LocalSettings.Values;            
            if (!settings.ContainsKey(key))
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                settings[key] = true;
            }            
        }

        private void DoUpdateTile()
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