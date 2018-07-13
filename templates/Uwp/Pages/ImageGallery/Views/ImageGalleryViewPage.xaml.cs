using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewPage : Page
    {
        public ImageGalleryViewPage()
        {
            InitializeComponent();
            ViewModel.Initialize(gridView);
        }
    }
}
