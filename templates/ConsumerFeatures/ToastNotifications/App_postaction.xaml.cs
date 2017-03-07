namespace RootNamespace
{
    sealed partial class App : Application
    {
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            //^^            
            Services.ToastNotificationsService.ShowToastNotificationSample();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            Services.ToastNotificationsService.HandleNotificationActivation(args);
        }
    }
}