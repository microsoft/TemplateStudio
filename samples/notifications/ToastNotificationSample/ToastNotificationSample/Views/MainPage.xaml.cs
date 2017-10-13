using System;

using ToastNotificationSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ToastNotificationSample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
