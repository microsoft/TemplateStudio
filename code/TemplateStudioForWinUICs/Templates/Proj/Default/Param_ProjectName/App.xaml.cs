using Microsoft.UI.Xaml;
using Param_RootNamespace.Helpers;
using WinUIEx;

// To learn more about WinUI3, see: https://docs.microsoft.com/windows/apps/winui/winui3/.
namespace Param_RootNamespace;

public partial class App : Application
{
    public static WindowEx MainWindow { get; } = new() { Backdrop = new MicaSystemBackdrop(), Content = null, PersistenceId = "DA786591-C56F-426F-8ECA-CA016C49F8A2", Title = "AppDisplayName".GetLocalized() };

    public App()
    {
        InitializeComponent();

        UnhandledException += App_UnhandledException;
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
    }
}
