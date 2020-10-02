using System.IO;
using System.Reflection;
using DotNetCoreWpfApp.Contracts.Services;
using DotNetCoreWpfApp.Core.Contracts.Services;
using DotNetCoreWpfApp.Core.Services;
using DotNetCoreWpfApp.Models;
using DotNetCoreWpfApp.Services;
using DotNetCoreWpfApp.ViewModels;
using DotNetCoreWpfApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace DotNetCoreWpfApp.Tests.NUnit
{
    public class PagesTests
    {
        private IHost _host;

        [SetUp]
        public void Setup()
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(c => c.SetBasePath(appLocation))
                .ConfigureServices(ConfigureServices)
                .Build();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Services
            services.AddSingleton<ISystemService, SystemService>();
            services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();

            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<ContentGridViewModel>();
            services.AddTransient<ContentGridDetailViewModel>();
            services.AddTransient<DataGridViewModel>();
            services.AddTransient<MasterDetailViewModel>();
            services.AddTransient<SettingsViewModel>();

            // Configuration
            services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
        }

        // TODO WTS: Add tests for functionality you add to MainViewModel.
        [Test]
        public void TestMainViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(MainViewModel));

            Assert.IsNotNull(vm);
        }

        [Test]
        public void TestGetMainPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(MainViewModel).FullName);
                Assert.AreEqual(typeof(MainPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to ContentGridViewModel.
        [Test]
        public void TestContentGridViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(ContentGridViewModel));

            Assert.IsNotNull(vm);
        }

        [Test]
        public void TestGetContentGridPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(ContentGridViewModel).FullName);
                Assert.AreEqual(typeof(ContentGridPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to ContentGridDetailViewModel.
        [Test]
        public void TestContentGridDetailViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(ContentGridDetailViewModel));

            Assert.IsNotNull(vm);
        }

        [Test]
        public void TestGetContentGridDetailPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(ContentGridDetailViewModel).FullName);
                Assert.AreEqual(typeof(ContentGridDetailPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to DataGridViewModel.
        [Test]
        public void TestDataGridViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(DataGridViewModel));

            Assert.IsNotNull(vm);
        }

        [Test]
        public void TestGetDataGridPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(DataGridViewModel).FullName);
                Assert.AreEqual(typeof(DataGridPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to MasterDetailViewModel.
        [Test]
        public void TestMasterDetailViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(MasterDetailViewModel));

            Assert.IsNotNull(vm);
        }

        [Test]
        public void TestGetMasterDetailPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(MasterDetailViewModel).FullName);
                Assert.AreEqual(typeof(MasterDetailPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to XAMLIslandViewModel.
        [Test]
        public void TestXamlIslandViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(XAMLIslandViewModel));
            Assert.IsNotNull(vm);
        }

        [Test]
        public void TestGetXamlIslandPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(XAMLIslandViewModel).FullName);
                Assert.AreEqual(typeof(XAMLIslandPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to SettingsViewModel.
        [Test]
        public void TestSettingsViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(SettingsViewModel));

            Assert.IsNotNull(vm);
        }

        [Test]
        public void TestGetSettingsPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(SettingsViewModel).FullName);
                Assert.AreEqual(typeof(SettingsPage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }
    }
}
