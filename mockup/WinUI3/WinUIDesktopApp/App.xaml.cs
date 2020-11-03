using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;

using WinUIDesktopApp.Activation;
using WinUIDesktopApp.Contracts.Services;
using WinUIDesktopApp.Contracts.Views;
using WinUIDesktopApp.Core.Contracts.Services;
using WinUIDesktopApp.Core.Services;
using WinUIDesktopApp.Services;
using WinUIDesktopApp.ViewModels;
using WinUIDesktopApp.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace WinUIDesktopApp
{
    public partial class App : Application
    {
        public static Window CurrentWindow { get; set; }

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;
            Ioc.Default.ConfigureServices(ConfigureServices);
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            var activationService = Ioc.Default.GetService<IActivationService>();
            await activationService.ActivateAsync(args);
        }

        protected override async void OnActivated(Windows.ApplicationModel.Activation.IActivatedEventArgs args)
        {
            base.OnActivated(args);
            var activationService = Ioc.Default.GetService<IActivationService>();
            await activationService.ActivateAsync(args);
        }

        private void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers
            services.AddTransient<IActivationHandler, ToastNotificationsActivationHandler>();

            // Services            
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddTransient<IToastNotificationsService, ToastNotificationsService>();

            // Core Services
            services.AddSingleton<ISampleDataService, SampleDataService>();

            // Views and ViewModels
            services.AddTransient<IShellWindow, ShellWindow>();
            services.AddTransient<ShellViewModel>();

            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();

            services.AddTransient<ContentGridViewModel>();
            services.AddTransient<ContentGridPage>();

            services.AddTransient<ContentGridDetailViewModel>();
            services.AddTransient<ContentGridDetailPage>();

            services.AddTransient<DataGridViewModel>();
            services.AddTransient<DataGridPage>();

            services.AddTransient<WebViewViewModel>();
            services.AddTransient<WebViewPage>();

            services.AddTransient<FormViewModel>();
            services.AddTransient<FormPage>();

            services.AddTransient<MasterDetailViewModel>();
            services.AddTransient<MasterDetailPage>();

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
        }
    }
}
