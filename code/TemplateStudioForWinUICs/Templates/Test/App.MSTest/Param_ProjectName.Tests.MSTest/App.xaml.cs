using Microsoft.UI.Xaml;

namespace Param_RootNamespace.Tests.MSTest;

public partial class App : Application
{
    public static Window MainWindow { get; } = new MainWindow();

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        MainWindow.Activate();
    }
}
