using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Windows.UI.Xaml.Controls;
using Prism.Windows.Navigation;
using WTSPrismNavigationBase.Views;
using WTSPrismNavigationBase.Services;
using WTSPrism.Constants;

namespace WTSPrismNavigationBase
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : PrismUnityApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterType<ILocationService, LocationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISampleDataService, SampleDataService>();
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
        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await ThemeSelectorService.InitializeAsync();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            Services.ThemeSelectorService.SetRequestedTheme();

            NavigationService.Navigate(PageTokens.MainPage, null);
            Window.Current.Activate();
            return Task.CompletedTask;
        }
    }
}
