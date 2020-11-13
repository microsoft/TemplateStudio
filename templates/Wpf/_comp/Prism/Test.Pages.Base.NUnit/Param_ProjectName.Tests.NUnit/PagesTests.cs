using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Prism.Regions;
using Unity;
using NUnit.Framework;
using Param_RootNamespace.Models;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Tests.NUnit
{
    public class PagesTests
    {
        private IUnityContainer _container;

        [SetUp]
        public void Setup()
        {
            _container = new UnityContainer();
            _container.RegisterType<IRegionManager, RegionManager>();

            // Core Services

            // App Services

            // Configuration
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddJsonFile("appsettings.json")
                .Build();
            var appConfig = configuration
                .GetSection(nameof(AppConfig))
                .Get<AppConfig>();

            // Register configurations to IoC
            _container.RegisterInstance(configuration);
            _container.RegisterInstance(appConfig);
        }
    }
}