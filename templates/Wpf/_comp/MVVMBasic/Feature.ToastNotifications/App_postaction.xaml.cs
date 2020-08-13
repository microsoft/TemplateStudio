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
//^^
//{[{
        public async Task StartAsync()
            => await _host.StartAsync();
//}]}

        private async void OnStartup(object sender, StartupEventArgs e)
        {
//{[{
            // Read more about sending local toast notifications from desktop C# apps
            // https://docs.microsoft.com/windows/uwp/design/shell/tiles-and-notifications/send-local-toast-desktop
            //
            // Register AUMID, COM server, and activator
            DesktopNotificationManagerCompat.RegisterAumidAndComServer<ToastNotificationActivator>("Param_ProjectName");
            DesktopNotificationManagerCompat.RegisterActivator<ToastNotificationActivator>();

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
            if (e.Args.Contains(DesktopNotificationManagerCompat.ToastActivatedLaunchArg))
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
