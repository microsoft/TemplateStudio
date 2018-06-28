using System;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using wts.ItemName.ViewModels;

namespace wts.ItemName.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public Frame ShellFrame => shellFrame;

        public ShellPage()
        {
            InitializeComponent();
            HideNavViewBackButton();
        }

        public void SetRootFrame(Frame frame)
        {
            shellFrame.Content = frame;
            ViewModel.Initialize(frame, navigationView);
        }

        private void HideNavViewBackButton()
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6))
            {
                navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            }
        }
    }
}
