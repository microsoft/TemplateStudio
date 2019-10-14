//{[{
using System.IO;
using System.Reflection;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Extensions.Configuration;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace
{
    public partial class App : Application
    {
//{[{
        private IApplicationHostService _host;

        public ViewModelLocator Locator
            => Resources["Locator"] as ViewModelLocator;
//}]}

        public App()
        {
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
//{[{
            AddConfiguration(e.Args);
            _host = SimpleIoc.Default.GetInstance<IApplicationHostService>();
            await _host.StartAsync();
//}]}
        }

//{[{
        private void AddConfiguration(string[] args)
        {
            var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(appLocation)
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .Build();

            Locator.AddConfiguration(configuration);
        }
//}]}
        private async void OnExit(object sender, ExitEventArgs e)
        {
//{[{
            await _host.StopAsync();
            _host = null;
//}]}
        }
    }
}