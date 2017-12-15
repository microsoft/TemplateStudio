using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using WTSGeneratedPivot.Helpers;
using WTSGeneratedPivot.Models;
using WTSGeneratedPivot.Services;
using WTSGeneratedPivot.Views;

namespace WTSGeneratedPivot.ViewModels
{
    public class ImageGalleryViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly ISampleDataService sampleDataService;

        public const string ImageGallerySelectedImageId = "ImageGallerySelectedImageId";
        public const string ImageGalleryAnimationOpen = "ImageGallery_AnimationOpen";
        public const string ImageGalleryAnimationClose = "ImageGallery_AnimationClose";

        private ObservableCollection<SampleImage> _source;
        private ICommand _itemSelectedCommand;
        private GridView _imagesGridView;

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new DelegateCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance)
        {
            navigationService = navigationServiceInstance;

            // TODO WTS: Replace this with your actual data
            sampleDataService = sampleDataServiceInstance;
            Source = sampleDataService.GetGallerySampleData();
        }

        public async Task LoadAnimationAsync(GridView imagesGridView)
        {
            _imagesGridView = imagesGridView;
            var selectedImageId = await ApplicationData.Current.LocalSettings.ReadAsync<string>(ImageGallerySelectedImageId);
            if (!string.IsNullOrEmpty(selectedImageId))
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryAnimationClose);
                if (animation != null)
                {
                    var item = _imagesGridView.Items.FirstOrDefault(i => ((SampleImage)i).ID == selectedImageId);
                    _imagesGridView.ScrollIntoView(item);
                    await _imagesGridView.TryStartConnectedAnimationAsync(animation, item, "galleryImage");
                }

                ApplicationData.Current.LocalSettings.SaveString(ImageGallerySelectedImageId, string.Empty);
            }
        }

        private void OnsItemSelected(ItemClickEventArgs args)
        {
            var selected = args.ClickedItem as SampleImage;
            _imagesGridView.PrepareConnectedAnimation(ImageGalleryAnimationOpen, selected, "galleryImage");
            navigationService.Navigate(PageTokens.ImageGalleryDetailPage, args.ClickedItem);
        }
    }
}
