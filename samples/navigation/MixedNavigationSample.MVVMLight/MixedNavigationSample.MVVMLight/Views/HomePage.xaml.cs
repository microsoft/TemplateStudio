using System;

using MixedNavigationSample.MVVMLight.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.MVVMLight.Views
{
    public sealed partial class HomePage : Page
    {
        private HomeViewModel ViewModel
        {
            get { return DataContext as HomeViewModel; }
        }

        public HomePage()
        {
            InitializeComponent();
        }
    }
}
