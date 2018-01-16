using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        public const string ImageGalleryViewSelectedIdKey = "ImageGalleryViewSelectedIdKey";
        public const string ImageGalleryViewAnimationOpen = "ImageGalleryView_AnimationOpen";
        public const string ImageGalleryViewAnimationClose = "ImageGalleryView_AnimationClose";

        private ObservableCollection<SampleImage> _source;

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Param_Setter(ref _source, value);
        }

        public ImageGalleryViewPage()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
            InitializeComponent();
        }

        private void ImagesGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selected = e.ClickedItem as SampleImage;
            ImagesGridView.PrepareConnectedAnimation(ImageGalleryViewAnimationOpen, selected, "galleryImage");
            NavigationService.Navigate<ImageGalleryViewDetailPage>(e.ClickedItem);
        }

        private async void ImagesGridView_Loaded(object sender, RoutedEventArgs e)
        {
            var selectedImageId = await ApplicationData.Current.LocalSettings.ReadAsync<string>(ImageGalleryViewSelectedIdKey);
            if (!string.IsNullOrEmpty(selectedImageId))
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewAnimationClose);
                if (animation != null)
                {
                    var item = ImagesGridView.Items.FirstOrDefault(i => ((SampleImage)i).ID == selectedImageId);
                    ImagesGridView.ScrollIntoView(item);
                    await ImagesGridView.TryStartConnectedAnimationAsync(animation, item, "galleryImage");
                }

                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewSelectedIdKey, string.Empty);
            }
        }
    }
}
