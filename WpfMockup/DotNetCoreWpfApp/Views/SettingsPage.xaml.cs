using System.Windows.Controls;

using DotNetCoreWpfApp.ViewModels;

namespace DotNetCoreWpfApp.Views
{
    public partial class SettingsPage : Page
    {
        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
