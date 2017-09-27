using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class ImageGalleryViewDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
        private static UIElement _image;
        private object _selectedImage;
        private ObservableCollection<SampleImage> _source;
        private Visibility _flipViewVisibility = Visibility.Collapsed;
        private Visibility _preViewImageVisibility = Visibility.Visible;
        private Visibility _shapeVisibility = Visibility.Visible;

        public object SelectedImage
        {
            get => _selectedImage;
            set
            {
                Set(ref _selectedImage, value);
                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewViewModel.ImageGalleryViewSelectedImageId, ((SampleImage)SelectedImage).ID);
            }
        }

        public ObservableCollection<SampleImage> Source
        {
            get => _source;
            set => Set(ref _source, value);
        }

        public Visibility FlipViewVisibility
        {
            get => _flipViewVisibility;
            set => Set(ref _flipViewVisibility, value);
        }

        public Visibility PreViewImageVisibility
        {
            get => _preViewImageVisibility;
            set => Set(ref _preViewImageVisibility, value);
        }

        public Visibility ShapeVisibility
        {
            get => _shapeVisibility;
            set => Set(ref _shapeVisibility, value);
        }

        public ImageGalleryViewDetailViewModel()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
            _timer.Tick += Timer_Tick;
        }

        public void SetImage(UIElement image) => _image = image;

        public void Initialize(SampleImage sampleImage)
        {
            SelectedImage = Source.FirstOrDefault(i => i.ID == sampleImage.ID);
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewViewModel.ImageGalleryViewAnimationOpen);
            animation?.TryStart(_image);
            _timer.Start();
        }

        public void SetAnimation()
        {
            PreViewImageVisibility = Visibility.Visible;
            ConnectedAnimationService.GetForCurrentView()?.PrepareToAnimate(ImageGalleryViewViewModel.ImageGalleryViewAnimationClose, _image);
        }

        private async void Timer_Tick(object sender, object e)
        {
            _timer.Stop();
            FlipViewVisibility = Visibility.Visible;
            ShapeVisibility = Visibility.Collapsed;
            await Task.Delay(50);
            PreViewImageVisibility = Visibility.Collapsed;
        }
    }
}
