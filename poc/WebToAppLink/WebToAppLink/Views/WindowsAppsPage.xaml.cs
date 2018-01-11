using System;

using WebToAppLink.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WebToAppLink.Views
{
    public sealed partial class WindowsAppsPage : Page
    {
        public WindowsAppsViewModel ViewModel { get; } = new WindowsAppsViewModel();

        public WindowsAppsPage()
        {
            InitializeComponent();
        }
    }
}
