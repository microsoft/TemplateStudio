using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewPage : Page, IImageGalleryViewPage
    {
        public ImageGalleryViewPage()
        {
            InitializeComponent();
        }

        public GridView GetGridView() => gridView;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                await ViewModel.LoadAnimationAsync();
            }
        }
    }
}
