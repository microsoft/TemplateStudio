using Param_RootNamespace.Helpers;
using Microsoft.UI.Xaml.Media;

namespace Param_RootNamespace;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        this.SystemBackdrop = new MicaBackdrop();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
