using System.IO;
using System.Reflection;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.Models;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;
using Param_RootNamespace.Views;

namespace Param_RootNamespace.Tests.MSTest
{
    [TestClass]
    public class PagesTests
    {
        public PagesTests()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddJsonFile("appsettings.json")
                .Build();

            var appConfig = configuration
                .GetSection(nameof(AppConfig))
                .Get<AppConfig>();

            SimpleIoc.Default.Reset();

            // Register configurations to IoC
            SimpleIoc.Default.Register(() => configuration);
            SimpleIoc.Default.Register(() => appConfig);

            // App Host
            SimpleIoc.Default.Register<IApplicationHostService, ApplicationHostService>();

            // Core Services

            // Services
            SimpleIoc.Default.Register<IPageService, PageService>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();

            // Window
            SimpleIoc.Default.Register<IShellWindow, ShellWindow>();
            SimpleIoc.Default.Register<ShellViewModel>();

            // Pages
        }

        private void RegisterPage<VM, V>()
            where VM : ViewModelBase
            where V : Page
        {
            SimpleIoc.Default.Register<VM>();
            SimpleIoc.Default.Register<V>();
            SimpleIoc.Default.GetInstance<IPageService>().Configure<VM, V>();
        }
    }
}
