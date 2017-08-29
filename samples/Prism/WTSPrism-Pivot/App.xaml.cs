using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using WTSPrism.Services;

namespace WTSPrism
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : PrismUnityApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        public void RegisterTypes()
        {
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterType<ILocationService, LocationService>(new ContainerControlledLifetimeManager());
        }

        protected async override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            RegisterTypes();
            await ThemeSelectorService.InitializeAsync();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            Services.ThemeSelectorService.SetRequestedTheme();

            NavigationService.Navigate("Pivot", null);
            return Task.FromResult(true);
        }
    }
}
