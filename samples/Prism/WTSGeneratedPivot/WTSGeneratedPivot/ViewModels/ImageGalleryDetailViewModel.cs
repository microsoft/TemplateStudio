using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Prism.Windows.Mvvm;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

using WTSGeneratedPivot.Helpers;
using WTSGeneratedPivot.Models;
using WTSGeneratedPivot.Services;

namespace WTSGeneratedPivot.ViewModels
{
    public class ImageGalleryDetailViewModel : ViewModelBase
    {
        private readonly ISampleDataService sampleDataService;
        private static UIElement _image;
        private object _selectedImage;
        private ObservableCollection<SampleImage> _source;

        public object SelectedImage
        {
            get => _selectedImage;
            set
            {
                SetProperty(ref _selectedImage, value);
                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewModel.ImageGallerySelectedImageId, ((SampleImage)SelectedImage).ID);
            }
        }

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public ImageGalleryDetailViewModel(ISampleDataService sampleDataServiceInstance)
        {
            // TODO WTS: Replace this with your actual data
            sampleDataService = sampleDataServiceInstance;
            Source = sampleDataService.GetGallerySampleData();
        }

        public void SetImage(UIElement image) => _image = image;

        public void Initialize(SampleImage sampleImage)
        {
            SelectedImage = Source.FirstOrDefault(i => i.ID == sampleImage.ID);
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewModel.ImageGalleryAnimationOpen);
            animation?.TryStart(_image);
        }

        public void SetAnimation()
        {
            ConnectedAnimationService.GetForCurrentView()?.PrepareToAnimate(ImageGalleryViewModel.ImageGalleryAnimationClose, _image);
        }
    }
}
