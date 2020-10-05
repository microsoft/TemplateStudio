//{[{
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.Notifications;
using Param_RootNamespace.Activation;
//}]}
namespace Param_RootNamespace
{
    public partial class App : Application
    {

        private async void OnStartup(object sender, StartupEventArgs e)
        {
//{[{
            // https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast
            ToastNotificationManagerCompat.OnActivated += (toastArgs) =>
            {
                Current.Dispatcher.Invoke(async () =>
                {
                    var config = SimpleIoc.Default.GetInstance<IConfiguration>();
                    config[ToastNotificationActivationHandler.ActivationArguments] = toastArgs.Argument;
                    await _host.StartAsync();
                });
            };
//}]}
            _host = SimpleIoc.Default.GetInstance<IApplicationHostService>();
//^^
//{[{
            if (ToastNotificationManagerCompat.WasCurrentProcessToastActivated())
            {
                // ToastNotificationActivator code will run after this completes and will show a window if necessary.
                return;
            }
//}]}
            await _host.StartAsync();
        }

        private void AddConfiguration(string[] args)
        {
//{[{
            // TODO: Register arguments you want to use on App initialization
            var activationArgs = new Dictionary<string, string>
            {
                { ToastNotificationActivationHandler.ActivationArguments, string.Empty},
            };
//}]}

            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddCommandLine(args)
//{[{
                .AddInMemoryCollection(activationArgs)
//}]}
                .AddJsonFile("appsettings.json")
                .Build();

            Locator.AddConfiguration(configuration);
        }
    }
}
