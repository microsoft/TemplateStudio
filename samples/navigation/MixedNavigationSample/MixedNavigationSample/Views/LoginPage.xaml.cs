using System;

using MixedNavigationSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginViewModel ViewModel { get; } = new LoginViewModel();

        public LoginPage()
        {
            InitializeComponent();
        }
    }
}
