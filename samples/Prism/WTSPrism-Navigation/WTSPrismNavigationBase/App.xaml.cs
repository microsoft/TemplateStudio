using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;

using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Mvvm;
using System.Globalization;
using Windows.UI.Xaml.Controls;
using Prism.Windows.Navigation;
using WTSPrismNavigationBase.Interfaces;
using WTSPrismNavigationBase.Views;
using Microsoft.Toolkit.Uwp.UI.Controls;
using WTSPrismNavigationBase.ViewModels;
using WTSPrismNavigationBase.Services;

namespace WTSPrismNavigationBase
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : PrismUnityApplication
    {

        protected async override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            RegisterTypes();
            await ThemeSelectorService.InitializeAsync();
        }

        private void RegisterTypes()
        {
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterType<ILocationService, LocationService>(new ContainerControlledLifetimeManager());
        }

        public void SetNavigationFrame(Frame frame)
        {
            var sessionStateService = Container.Resolve<ISessionStateService>();
            base.CreateNavigationService(new FrameFacadeAdapter(frame), sessionStateService);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<ShellPage>();
            shell.SetRootFrame(rootFrame);
            return shell;
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            Services.ThemeSelectorService.SetRequestedTheme();

            NavigationService.Navigate("Main", null);
            Window.Current.Activate();
            return Task.FromResult<object>(null);
        }
    }
}
