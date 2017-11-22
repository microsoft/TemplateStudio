using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Param_ItemNamespace.Extensions;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ImageGalleryViewDetailPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private DispatcherTimer _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(500) };
        private DataTransferManager _dataTransferManager;
        private object _selectedImage;
        private ObservableCollection<SampleImage> _source;

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

        public ImageGalleryViewDetailPage()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
            InitializeComponent();
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var sampleImage = e.Parameter as SampleImage;
            SelectedImage = Source.FirstOrDefault(i => i.ID == sampleImage.ID);
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewPage.ImageGalleryViewAnimationOpen);
            animation?.TryStart(previewImage);
            showFlipView.Begin();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                previewImage.Visibility = Visibility.Visible;
                ConnectedAnimationService.GetForCurrentView()?.PrepareToAnimate(ImageGalleryViewPage.ImageGalleryViewAnimationClose, previewImage);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        // TODO WTS: Use Share in ImageGalleryViewDetailPage to share the SelectedImage with other apps.
        private void Share()
        {
            DataTransferManager.ShowShareUI();
        }

        private async void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            // This event will be fired when the share operation starts.
            // We need to add data to DataRequestedEventArgs through SetData extension method
            var config = new ShareSourceData("Sharing an image");

            // TODO WTS: Use ShareSourceConfig instance to set the data you want to share
            StorageFile image = await StorageFile.GetFileFromApplicationUriAsync(new Uri(((SampleImage)SelectedImage).Source));
            config.SetImage(image);

            args.Request.SetData(config);
            args.Request.Data.ShareCompleted += OnShareCompleted;
        }

        private void OnShareCompleted(DataPackage sender, ShareCompletedEventArgs args)
        {
            // This event will be fired when Share Operation will finish
            // TODO WTS: If you need to handle any action when de data is shared implement on this method
            sender.ShareCompleted -= OnShareCompleted;
        }
    }
}
