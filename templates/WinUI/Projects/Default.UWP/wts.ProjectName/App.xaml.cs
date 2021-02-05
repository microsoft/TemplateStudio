using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Param_RootNamespace
{
    public sealed partial class App : Application
    {
        public static Window MainWindow => Window.Current;

        public App()
        {
            InitializeComponent();
            UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
        }

        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
        }

        protected override async void OnActivated(Windows.ApplicationModel.Activation.IActivatedEventArgs args)
        {
            base.OnActivated(args);
        }
    }
}
