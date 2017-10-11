using Windows.UI.Xaml;
//^^
//{[{
using Param_RootNamespace.Services;
//}]}

namespace Param_RootNamespace
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
//{[{
            Container.RegisterInstance<IToastNotificationsService>(new ToastNotificationsService());
//}]}
        }

        private async Task LaunchApplication(string page, object launchParam)
        {
            NavigationService.Navigate(page, launchParam);            
            Window.Current.Activate();
//{[{
            Container.Resolve<IToastNotificationsService>().ShowToastNotificationSample();
//}]}
            await Task.CompletedTask;
        }

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
//{[{
            if (args.Kind == ActivationKind.ToastNotification && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and the user launched the app from a toast notification
                OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
            }
//}]}
            return Task.CompletedTask;
        }
    }
}
