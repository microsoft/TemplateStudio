using Microsoft.UI.Xaml;
using Param_RootNamespace.Helpers;

// To learn more about WinUI3, see: https://docs.microsoft.com/windows/apps/winui/winui3/.
namespace Param_RootNamespace
{
    public partial class App : Application
    {
        public static Window MainWindow { get; set; } = new Window() { Title = "AppDisplayName".GetLocalized() };

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
            UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/windows/winui/api/microsoft.ui.xaml.unhandledexceptioneventargs
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
        }

        protected override async void OnActivated(Windows.ApplicationModel.Activation.IActivatedEventArgs args)
        {
            base.OnActivated(args);
        }

        private void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
        }
    }
}