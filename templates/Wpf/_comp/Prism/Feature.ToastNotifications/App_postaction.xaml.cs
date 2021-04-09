//{[{
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Notifications;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
//{[{
        public const string ToastNotificationActivationArguments = "ToastNotificationActivationArguments";

//}]}
        private string[] _startUpArgs;

        protected override async void OnInitialized()
        {
//{[{
            // https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast
            ToastNotificationManagerCompat.OnActivated += (toastArgs) =>
            {
                Current.Dispatcher.Invoke(async () =>
                {
                    var config = Container.Resolve<IConfiguration>();

                    // Store ToastNotification arguments in configuration, so you can use them from any point in the app
                    config[App.ToastNotificationActivationArguments] = toastArgs.Argument;

                    App.Current.MainWindow.Activate();
                    if (App.Current.MainWindow.WindowState == WindowState.Minimized)
                    {
                        App.Current.MainWindow.WindowState = WindowState.Normal;
                    }

                    await Task.CompletedTask;
                });
            };

            var toastNotificationsService = Container.Resolve<IToastNotificationsService>();
            toastNotificationsService.ShowToastNotificationSample();
//}]}

//^^
//{[{
            if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated())
            {
                // ToastNotificationActivator code will run after this completes and will show a window if necessary.
                return;
            }
//}]}
            base.OnInitialized();
            await Task.CompletedTask;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // App Services
//{[{
            containerRegistry.RegisterSingleton<IToastNotificationsService, ToastNotificationsService>();
//}]}
        }

        private IConfiguration BuildConfiguration()
        {
//{[{
            // TODO: Register arguments you want to use on App initialization
            var activationArgs = new Dictionary<string, string>
            {
                { ToastNotificationActivationArguments, string.Empty }
            };
//}]}

            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            return new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddJsonFile("appsettings.json")
                .AddCommandLine(_startUpArgs)
//{[{
                .AddInMemoryCollection(activationArgs)
//}]}
                .Build();
        }
    }
}
