using System;

using MixedNavigationSample.MVVMBasic.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.MVVMBasic.Views
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
