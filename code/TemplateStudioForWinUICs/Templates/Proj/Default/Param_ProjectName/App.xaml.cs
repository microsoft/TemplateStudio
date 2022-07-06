using Microsoft.UI.Xaml;
using Param_RootNamespace.Helpers;
using WinUIEx;

namespace Param_RootNamespace;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    public static WindowEx MainWindow { get; } = InitializeMainWindow();

    private static WindowEx InitializeMainWindow()
    {
        return new WindowEx()
        {
            Backdrop = new MicaSystemBackdrop(),
            Content = null,
            MinHeight = 500,
            MinWidth = 500,
            PersistenceId = "MainWindow",
            Title = "AppDisplayName".GetLocalized()
        };
    }

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
