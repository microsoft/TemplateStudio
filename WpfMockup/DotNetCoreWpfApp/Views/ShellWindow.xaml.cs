using System.Windows.Controls;

using DotNetCoreWpfApp.Contracts.Views;
using DotNetCoreWpfApp.ViewModels;

using MahApps.Metro.Controls;

namespace DotNetCoreWpfApp.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow
    {
        public ShellWindow(ShellViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public Frame GetNavigationFrame()
            => shellFrame;

        public void ShowWindow()
            => Show();

        public void CloseWindow()
            => Close();
    }
}
