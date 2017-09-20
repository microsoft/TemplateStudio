using System;
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.SelectedImage = e.Parameter as SampleImage;
        }
    }
}
