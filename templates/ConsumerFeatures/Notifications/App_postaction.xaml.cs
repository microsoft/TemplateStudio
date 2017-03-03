namespace RootNamespace
{
    sealed partial class App : Application
    {
        public App()
        {
            //^^
            Services.LiveTileService.EnableQueue();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            //^^
            //Services.NotificationsFeatureService.InitializeNotificationsHub();
            Services.NotificationsFeatureService.InitializeStoreNotifications();
            Services.NotificationsFeatureService.ShowToastNotificationSample();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            Services.NotificationsService.HandleNotificationActivation(args);
        }
    }
}