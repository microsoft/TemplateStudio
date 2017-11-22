using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

using Param_ItemNamespace.Extensions;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class ImageGalleryViewDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private static UIElement _image;
        private object _selectedImage;
        private DataTransferManager _dataTransferManager;
        private ObservableCollection<SampleImage> _source;
        private ICommand _shareCommand;

        // TODO WTS: Use ShareCommand in ImageGalleryViewDetailPage to share the SelectedImage with other apps.
        public ICommand ShareCommand => _shareCommand ?? (_shareCommand = new RelayCommand(OnShare));

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

        public ImageGalleryViewDetailViewModel()
        {
            // TODO WTS: Replace this with your actual data
            Source = SampleDataService.GetGallerySampleData();
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        public void SetImage(UIElement image) => _image = image;

        public void Initialize(SampleImage sampleImage)
        {
            SelectedImage = Source.FirstOrDefault(i => i.ID == sampleImage.ID);
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewViewModel.ImageGalleryViewAnimationOpen);
            animation?.TryStart(_image);
        }

        public void SetAnimation()
        {
            ConnectedAnimationService.GetForCurrentView()?.PrepareToAnimate(ImageGalleryViewViewModel.ImageGalleryViewAnimationClose, _image);
        }

        public void RegisterEvents()
        {
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        public void UnregisterEvents()
        {
            _dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        private void OnShare()
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
