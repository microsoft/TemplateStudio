using Microsoft.UI.Xaml;
using Param_RootNamespace.Helpers;
using WinUIEx;

// To learn more about WinUI3, see: https://docs.microsoft.com/windows/apps/winui/winui3/.
namespace Param_RootNamespace;

public partial class App : Application
{
    public static WindowEx MainWindow { get; } = new();

    public App()
    {
        InitializeComponent();

        MainWindow.Backdrop = new MicaSystemBackdrop();
        MainWindow.Content = null;
        MainWindow.MinHeight = 500;
        MainWindow.MinWidth = 500;
        MainWindow.PersistenceId = "DA786591-C56F-426F-8ECA-CA016C49F8A2";
        MainWindow.Title = "AppDisplayName".GetLocalized();

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
