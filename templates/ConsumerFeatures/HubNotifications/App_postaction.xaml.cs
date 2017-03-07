namespace RootNamespace
{
    sealed partial class App : Application
    {
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            //^^            
            //Services.HubNotificationsService.Initialize();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            Services.HubNotificationsService.HandleNotificationActivation(args);
        }
    }
}