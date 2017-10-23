using System;

using MixedNavigationSample.MVVMLight.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.MVVMLight.Views
{
    public sealed partial class StartPage : Page
    {
        private StartViewModel ViewModel
        {
            get { return DataContext as StartViewModel; }
        }

        public StartPage()
        {
            InitializeComponent();
        }
    }
}
