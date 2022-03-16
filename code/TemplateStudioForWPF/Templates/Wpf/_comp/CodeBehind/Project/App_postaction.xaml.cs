//{[{
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.Models;
using Param_RootNamespace.Services;
using Param_RootNamespace.Views;
//}]}

namespace Param_RootNamespace
{
    public partial class App : Application
    {
//{[{
        private IHost _host;

        public T GetService<T>()
            where T : class
            => _host.Services.GetService(typeof(T)) as T;
//}]}

        public App()
        {
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
//{[{
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            // For more information about .NET generic host see  https://docs.microsoft.com/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.0
            _host = Host.CreateDefaultBuilder(e.Args)
                    .ConfigureAppConfiguration(c =>
                    {
                        c.SetBasePath(appLocation);
                    })
                    .ConfigureServices(ConfigureServices)
                    .Build();

            await _host.StartAsync();
//}]}
        }

//{[{
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // TODO WTS: Register your services, viewmodels and pages here

            // App Host
            services.AddHostedService<ApplicationHostService>();

            // Activation Handlers

            // Core Services

            // Services
            services.AddSingleton<INavigationService, NavigationService>();

            // Views
            services.AddTransient<IShellWindow, ShellWindow>();

            // Configuration
            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        }
//}]}
        private async void OnExit(object sender, ExitEventArgs e)
        {
//{[{
            await _host.StopAsync();
            _host.Dispose();
            _host = null;
//}]}
        }
    }
}