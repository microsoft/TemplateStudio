using MahApps.Metro.Controls;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views;

public partial class LogInWindow : MetroWindow, ILogInWindow
{
    public LogInWindow(LogInViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}
