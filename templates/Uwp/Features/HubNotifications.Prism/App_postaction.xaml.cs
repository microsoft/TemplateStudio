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
            Container.RegisterType<IHubNotificationsFeatureService, HubNotificationsFeatureService>(new ContainerControlledLifetimeManager());
//}]}
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            NavigationService.Navigate(page, launchParam);            
            Window.Current.Activate();
//{[{
            await Container.Resolve<IHubNotificationsFeatureService>().InitializeAsync();
//}]}
            await Task.CompletedTask;
        }

        protected override async Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
//{[{
            if (args.Kind == ActivationKind.ToastNotification && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Handle an Azure hub notification here
                // Since dev center, toast, and Azure notification hub will all active with an ActivationKind.ToastNotification
                // you may have to parse the toast data to determine where it came from and what action you want to take
                // If the app isn't running then launch the app here
                await OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
            }
//}]}

            await Task.CompletedTask;
        }
    }
}
