using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    public sealed partial class ImageGalleryViewPage : Page
    {
        public ImageGalleryViewPage()
        {
            InitializeComponent();
            Loaded += ImageGalleryViewPage_Loaded;
        }

        private async void ImageGalleryViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync();
        }
    }
}