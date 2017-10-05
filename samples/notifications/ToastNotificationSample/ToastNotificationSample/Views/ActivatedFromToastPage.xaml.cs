using System;

using ToastNotificationSample.ViewModels;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ToastNotificationSample.Views
{
    public sealed partial class ActivatedFromToastPage : Page
    {
        public ActivatedFromToastViewModel ViewModel { get; } = new ActivatedFromToastViewModel();

        public ActivatedFromToastPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(e.Parameter as ToastNotificationActivatedEventArgs);
        }
    }
}
