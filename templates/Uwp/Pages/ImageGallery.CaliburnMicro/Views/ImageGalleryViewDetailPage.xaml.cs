using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.ViewModels;
using Param_ItemNamespace.Core.Services;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewDetailPage : Page
    {
        public ImageGalleryViewDetailPage()
        {
            InitializeComponent();
        }

        private ImageGalleryViewDetailViewModel ViewModel
        {
            get { return DataContext as ImageGalleryViewDetailViewModel; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(e.Parameter as SampleImage, e.NavigationMode);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModel.UpdateConnectedAnimation();
            }
        }
    }
}
