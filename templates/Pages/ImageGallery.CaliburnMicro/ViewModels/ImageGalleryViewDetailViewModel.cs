using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;

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
    public class ImageGalleryViewDetailViewModel : Screen
    {
        private static UIElement _image;
        private DataTransferManager _dataTransferManager;
        private SampleImage _selectedImage;

        public SampleImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                Set(ref _selectedImage, value);
                ApplicationData.Current.LocalSettings.SaveString(ImageGalleryViewViewModel.ImageGalleryViewSelectedImageId, SelectedImage.ID);
            }
        }

        public string ID { get; set; }

        public BindableCollection<SampleImage> Source { get; } = new BindableCollection<SampleImage>();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // TODO WTS: Replace this with your actual data
            Source.AddRange(SampleDataService.GetGallerySampleData());
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
        }

        public void Initialize(UIElement image)
        {
            _image = image;
            SelectedImage = Source.FirstOrDefault(i => i.ID == ID);
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation(ImageGalleryViewViewModel.ImageGalleryViewAnimationOpen);
            animation?.TryStart(_image);
        }

        public void Share() => DataTransferManager.ShowShareUI();

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
