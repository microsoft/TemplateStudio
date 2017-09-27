using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewDetailPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
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
                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewPage.ImageGalleryViewSelectedImageId, ((SampleImage)SelectedImage).ID);
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

        public ImageGalleryViewDetailPage()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
            _timer.Tick += Timer_Tick;
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var sampleImage = e.Parameter as SampleImage;
            SelectedImage = Source.FirstOrDefault(i => i.ID == sampleImage.ID);
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewPage.ImageGalleryViewAnimationOpen);
            animation?.TryStart(previewImage);
            _timer.Start();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            PreViewImageVisibility = Visibility.Visible;
            ConnectedAnimationService.GetForCurrentView()?.PrepareToAnimate(ImageGalleryViewPage.ImageGalleryViewAnimationClose, previewImage);
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
