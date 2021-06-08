//{[{
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection;
using Param_RootNamespace.Activation;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;
using Param_RootNamespace.Views;
//}]}

namespace Param_RootNamespace
{
    public partial class App : Application
    {
        public App()
        {
//^^
//{[{
            Ioc.Default.ConfigureServices(ConfigureServices());
//}]}
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
//^^
//{[{
            var activationService = Ioc.Default.GetService<IActivationService>();
            await activationService.ActivateAsync(args);
//}]}
        }

//^^
//{[{
        private System.IServiceProvider ConfigureServices()
        {
            // TODO WTS: Register your services, viewmodels and pages here
            var services = new ServiceCollection();

            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services

            // Views and ViewModels
            return services.BuildServiceProvider();
        }
//}]}
    }
}

