using System;

using MixedNavigationSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.Views
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; } = new HomeViewModel();

        public HomePage()
        {
            InitializeComponent();
        }
    }
}
