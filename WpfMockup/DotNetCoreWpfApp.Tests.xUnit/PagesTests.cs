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
using Xunit;

namespace DotNetCoreWpfApp.Tests.xUnit
{
    public class PagesTests
    {
        private readonly IHost _host;

        public PagesTests()
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
        [Fact]
        public void TestMainViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(MainViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetMainPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(MainViewModel).FullName);
                Assert.Equal(typeof(MainPage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to ContentGridViewModel.
        [Fact]
        public void TestContentGridViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(ContentGridViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetContentGridPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(ContentGridViewModel).FullName);
                Assert.Equal(typeof(ContentGridPage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to ContentGridDetailViewModel.
        [Fact]
        public void TestContentGridDetailViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(ContentGridDetailViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetContentGridDetailPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(ContentGridDetailViewModel).FullName);
                Assert.Equal(typeof(ContentGridDetailPage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to DataGridViewModel.
        [Fact]
        public void TestDataGridViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(DataGridViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetDataGridPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(DataGridViewModel).FullName);
                Assert.Equal(typeof(DataGridPage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to MasterDetailViewModel.
        [Fact]
        public void TestMasterDetailViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(MasterDetailViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetMasterDetailPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(MasterDetailViewModel).FullName);
                Assert.Equal(typeof(MasterDetailPage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to XAMLIslandViewModel.
        [Fact]
        public void TestXamlIslandViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(XAMLIslandViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetXamlIslandPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(XAMLIslandViewModel).FullName);
                Assert.Equal(typeof(XAMLIslandPage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }

        // TODO WTS: Add tests for functionality you add to SettingsViewModel.
        [Fact]
        public void TestSettingsViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(SettingsViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetSettingsPageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(SettingsViewModel).FullName);
                Assert.Equal(typeof(SettingsPage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }
    }
}
