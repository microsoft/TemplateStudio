namespace RootNamespace
{
    sealed partial class App : Application
    {
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            //^^            
            Services.StoreNotificationsService.Initialize();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            Services.StoreNotificationsService.HandleNotificationActivation(args);
        }
    }
}