//{[{
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Param_RootNamespace.Activation;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;
using Param_RootNamespace.Views;
//}]}

namespace Param_RootNamespace
{
    public partial class App : Application
    {
//{[{
        // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Default Activation Handler
                services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

                // Other Activation Handlers

                // Services
                services.AddSingleton<IActivationService, ActivationService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // Core Services
                services.AddSingleton<IFileService, FileService>();

                // Views and ViewModels

                // Configuration
            })
            .Build();

        public static T GetService<T>()
            where T : class
            => _host.Services.GetService(typeof(T)) as T;
//}]}

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
//^^
//{[{
            var activationService = App.GetService<IActivationService>();
            await activationService.ActivateAsync(args);
//}]}
        }
    }
}

