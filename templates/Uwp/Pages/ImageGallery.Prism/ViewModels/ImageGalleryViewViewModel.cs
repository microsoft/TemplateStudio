using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

using Prism.Commands;
using Prism.Windows.Navigation;

namespace Param_ItemNamespace.ViewModels
{
    public class ImageGalleryViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;

        public const string ImageGalleryViewSelectedIdKey = "ImageGalleryViewSelectedIdKey";
        public const string ImageGalleryViewAnimationOpen = "ImageGalleryView_AnimationOpen";
        public const string ImageGalleryViewAnimationClose = "ImageGalleryView_AnimationClose";

        private ObservableCollection<SampleImage> _source;
        private ICommand _itemSelectedCommand;
        private GridView _imagesGridView;

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Param_Setter(ref _source, value);
        }

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new DelegateCommand<ItemClickEventArgs>(OnsItemSelected));

        public ImageGalleryViewViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance)
        {
            _navigationService = navigationServiceInstance;

            // TODO WTS: Replace this with your actual data
            _sampleDataService = sampleDataServiceInstance;
            Source = _sampleDataService.GetGallerySampleData();
        }

        public async Task LoadAnimationAsync(GridView imagesGridView)
        {
            _imagesGridView = imagesGridView;
            var selectedImageId = await ApplicationData.Current.LocalSettings.ReadAsync<string>(ImageGalleryViewSelectedIdKey);
            if (!string.IsNullOrEmpty(selectedImageId))
            {
                var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewAnimationClose);
                if (animation != null)
                {
                    var item = _imagesGridView.Items.FirstOrDefault(i => ((SampleImage)i).ID == selectedImageId);
                    _imagesGridView.ScrollIntoView(item);
                    await _imagesGridView.TryStartConnectedAnimationAsync(animation, item, "galleryImage");
                }

                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewSelectedIdKey, string.Empty);
            }
        }
    }
}
