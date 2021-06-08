using CommunityToolkit.Mvvm.DependencyInjection;

using Microsoft.UI.Xaml.Controls;

using WinUIMenuBarApp.ViewModels;

namespace WinUIMenuBarApp.Views
{
    public sealed partial class AppInfoPage : Page
    {
        public AppInfoViewModel ViewModel { get; }

        public AppInfoPage()
        {
            ViewModel = Ioc.Default.GetService<AppInfoViewModel>();
            InitializeComponent();
        }
    }
}
