using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;
using Prism.Windows.Navigation;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using WTSGeneratedNavigation.Services;
using WTSGeneratedNavigation.Views;

namespace WTSGeneratedNavigation
{
    public sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterType<IWhatsNewDisplayService, WhatsNewDisplayService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFirstRunDisplayService, FirstRunDisplayService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILiveTileService, LiveTileService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IStoreNotificationsService, StoreNotificationsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IHubNotificationsService, HubNotificationsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IToastNotificationsService, ToastNotificationsService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IBackgroundTaskService, BackgroundTaskService>(new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterType<IWebViewService, WebViewService>();
            Container.RegisterType<ISampleDataService, SampleDataService>();
            Container.RegisterType<ILocationService, LocationService>();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            return LaunchApplicationAsync(PageTokens.MainPage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            Services.ThemeSelectorService.SetRequestedTheme();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
            await Container.Resolve<IWhatsNewDisplayService>().ShowIfAppropriateAsync();
            await Container.Resolve<IFirstRunDisplayService>().ShowIfAppropriateAsync();
            Container.Resolve<ILiveTileService>().SampleUpdate();
            await Container.Resolve<IStoreNotificationsService>().InitializeAsync();
            await Container.Resolve<IHubNotificationsService>().InitializeAsync();
            Container.Resolve<IToastNotificationsService>().ShowToastNotificationSample();
            await Task.CompletedTask;
        }

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Protocol && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                var protocolArgs = args as ProtocolActivatedEventArgs;
                if (protocolArgs.Uri.AbsolutePath.Equals("sample", StringComparison.OrdinalIgnoreCase))
                {
                    var secret = "<<I-HAVE-NO-SECRETS>>";

                    try
                    {
                        if (protocolArgs.Uri.Query != null)
                        {
                            // The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
                            var decoder = new Windows.Foundation.WwwFormUrlDecoder(protocolArgs.Uri.Query);

                            secret = decoder.GetFirstValueByName("secret");
                        }
                    }
                    catch (Exception)
                    {
                        // NullReferenceException if the URI doesn't contain a query
                        // ArgumentException if the query doesn't contain a param called 'secret'
                    }

                    // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the URI used to launch
                    return LaunchApplicationAsync(PageTokens.UriSchemePage, secret);
                }
                else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
                {
                    // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                    OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
                }
            }

            if (args.Kind == ActivationKind.ToastNotification && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Handle a dev center notification here
                // Since dev center, toast, and Azure notification hub will all active with an ActivationKind.ToastNotification
                // you may have to parse the toast data to determine where it came from and what action you want to take
                // If the app isn't running then launch the app here
                OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
            }

            if (args.Kind == ActivationKind.ToastNotification && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Handle an Azure hub notification here
                // Since dev center, toast, and Azure notification hub will all active with an ActivationKind.ToastNotification
                // you may have to parse the toast data to determine where it came from and what action you want to take
                // If the app isn't running then launch the app here
                OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
            }

            if (args.Kind == ActivationKind.ToastNotification && args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Handle a toast notification here
                // Since dev center, toast, and Azure notification hub will all active with an ActivationKind.ToastNotification
                // you may have to parse the toast data to determine where it came from and what action you want to take
                // If the app isn't running then launch the app here
                OnLaunchApplicationAsync(args as LaunchActivatedEventArgs);
            }

            return Task.CompletedTask;
        }

        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);
            Container.Resolve<IBackgroundTaskService>().Start(args.TaskInstance);
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await Container.Resolve<ILiveTileService>().EnableQueueAsync().ConfigureAwait(false);
            Container.Resolve<IBackgroundTaskService>().RegisterBackgroundTasks();
            await ThemeSelectorService.InitializeAsync().ConfigureAwait(false);

            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "WTSGeneratedNavigation.ViewModels.{0}ViewModel, WTSGeneratedNavigation", viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
            await base.OnInitializeAsync(args);
        }

        public void SetNavigationFrame(Frame frame)
        {
            var sessionStateService = Container.Resolve<ISessionStateService>();
            CreateNavigationService(new FrameFacadeAdapter(frame), sessionStateService);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<ShellPage>();
            shell.SetRootFrame(rootFrame);
            return shell;
        }
    }
}
