//{[{
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using Param_RootNamespace.Activation;
using Param_RootNamespace.Contracts.Activation;
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
                    var config = GetService<IConfiguration>();
                    config[ToastNotificationActivationHandler.ActivationArguments] = toastArgs.Argument;
                    await _host.StartAsync();
                });
            };

            // TODO: Register arguments you want to use on App initialization
            var activationArgs = new Dictionary<string, string>
            {
                { ToastNotificationActivationHandler.ActivationArguments, string.Empty },
            };
//}]}
            _host = Host.CreateDefaultBuilder(e.Args)
                    .ConfigureAppConfiguration(c =>
                    {
//^^
//{[{
                        c.AddInMemoryCollection(activationArgs);
//}]}
                    })
                    .ConfigureServices(ConfigureServices)
                    .Build();
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

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Activation Handlers
//{[{
            services.AddSingleton<IActivationHandler, ToastNotificationActivationHandler>();
//}]}
            // Services
//{[{
            services.AddSingleton<IToastNotificationsService, ToastNotificationsService>();
//}]}
        }
    }
}
