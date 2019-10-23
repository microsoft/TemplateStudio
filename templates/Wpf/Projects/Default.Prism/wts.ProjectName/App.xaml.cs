using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Param_RootNamespace.Constants;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Models;
using Param_RootNamespace.ViewModels;
using Param_RootNamespace.Views;


namespace Param_RootNamespace
{
    public partial class App : PrismApplication
    {
        private string[] _startUpArgs;

        public App()
        {
        }

        protected override Window CreateShell()
            => Container.Resolve<ShellWindow>();

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _startUpArgs = e.Args;
            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Core Services
            containerRegistry.Register<IFilesService, FilesService>();

            // App Services

            // Views
            containerRegistry.RegisterForNavigation<ShellWindow>();

            // Configuration
            var configuration = BuildConfiguration();
            var appConfig = configuration
                .GetSection(nameof(AppConfig))
                .Get<AppConfig>();

            // Register configurations to IoC
            containerRegistry.RegisterInstance<IConfiguration>(configuration);
            containerRegistry.RegisterInstance<AppConfig>(appConfig);
        }

        private IConfiguration BuildConfiguration()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddJsonFile("appsettings.json")
                .AddCommandLine(_startUpArgs)
                .Build();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // We are remapping the default ViewName and ViewNameViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "Param_RootNamespace.ViewModels.{0}ViewModel, Param_RootNamespace", viewType.Name[0..^4]);
                return Type.GetType(viewModelName);
            });
            ViewModelLocationProvider.Register(typeof(ShellWindow).FullName, typeof(ShellViewModel));
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0

            // e.Handled = true;
        }
    }
}
