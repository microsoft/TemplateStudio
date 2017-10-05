using System;

using ToastNotificationSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ToastNotificationSample.Views
{
    public sealed partial class ActivatedFromToastPage : Page
    {
        public ActivatedFromToastViewModel ViewModel { get; } = new ActivatedFromToastViewModel();

        public ActivatedFromToastPage()
        {
            InitializeComponent();
        }
    }
}
