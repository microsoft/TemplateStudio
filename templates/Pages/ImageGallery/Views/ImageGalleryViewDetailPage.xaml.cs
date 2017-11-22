using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewDetailPage : Page
    {
        public ImageGalleryViewDetailPage()
        {
            InitializeComponent();
            ViewModel.SetImage(previewImage);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(e.Parameter as SampleImage);
            showFlipView.Begin();
            ViewModel.RegisterEvents();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                previewImage.Visibility = Visibility.Visible;
                ViewModel.SetAnimation();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.UnregisterEvents();
        }
    }
}
