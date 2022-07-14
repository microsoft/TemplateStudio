using Param_RootNamespace.Helpers;

namespace Param_RootNamespace;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
