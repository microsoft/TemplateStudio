using System;

using ShareSourceSample.Helpers;
using System.Windows.Input;
using ShareSourceSample.Services;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.ApplicationModel.DataTransfer;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;

namespace ShareSourceSample.ViewModels
{
    public class MainViewModel : Observable
    {
        private StorageFile _image;
        private ICommand _shareTextCommand;
        private ICommand _shareWebLinkCommand;
        private ICommand _shareHtmlCommand;
        private ICommand _shareImageCommand;
        private ICommand _shareDeferralImageCommand;
        private ICommand _shareApplicationLinkCommand;

        public ICommand ShareTextCommand => _shareTextCommand ?? (_shareTextCommand = new RelayCommand(OnShareText));

        public ICommand ShareWebLinkCommand => _shareWebLinkCommand ?? (_shareWebLinkCommand = new RelayCommand(OnShareWebLink));

        public ICommand ShareHtmlCommand => _shareHtmlCommand ?? (_shareHtmlCommand = new RelayCommand(OnShareHtml));

        public ICommand ShareImageCommand => _shareImageCommand ?? (_shareImageCommand = new RelayCommand(OnShareImage));

        public ICommand ShareDeferralImageCommand => _shareDeferralImageCommand ?? (_shareDeferralImageCommand = new RelayCommand(OnShareDeferralImage));

        public ICommand ShareApplicationLinkCommand => _shareApplicationLinkCommand ?? (_shareApplicationLinkCommand = new RelayCommand(OnShareApplicationLink));

        public MainViewModel()
        {
        }

        private void OnShareText()
        {
            ShareService.ShareText("Lorem ipsum", "Share text", "We are sharing text");
        }

        private void OnShareWebLink()
        {
            ShareService.ShareWebLink(new Uri("http://www.microsoft.com/"), "Share web link", "We are sharing web link");
        }

        private void OnShareHtml()
        {
            var html = "<html><body><h1>Lorem ipsum</h1><h2>hello world</h2><p>Excepteur sint occaecat cupidatat non proident sunt in culpa.</p></body></html>";
            ShareService.ShareHtml(html, "Share Html", "We are sharing Html");
        }

        private async void OnShareImage()
        {
            _image = await GetImage();
            if (_image != null)
            {
                ShareService.ShareImage(_image, "Share image", "We are sharing an image");
            }
        }

        private async void OnShareDeferralImage()
        {
            _image = await GetImage();
            if (_image != null)
            {
                ShareService.ShareDeferredContent(StandardDataFormats.Bitmap, GetDeferredImageAsync, "Share image", "We are sharing an image");
            }
        }

        private void OnShareApplicationLink()
        {
            ShareService.ShareApplicationLink(new Uri("my-app-sharesourcesample:navigate?page=MainPage"), "Share application link", "We are sharing application link");
            //ShareService.ShareApplicationLink(new Uri("picturesLibrary"), "Share application link", "We are sharing application link");
        }

        private async Task<object> GetDeferredImageAsync()
        {
            InMemoryRandomAccessStream inMemoryStream = new InMemoryRandomAccessStream();
            IRandomAccessStream imageStream = await _image.OpenAsync(FileAccessMode.Read);
            var imageDecoder = await BitmapDecoder.CreateAsync(imageStream);
            var imageEncoder = await BitmapEncoder.CreateForTranscodingAsync(inMemoryStream, imageDecoder);
            imageEncoder.BitmapTransform.ScaledWidth = (uint)(imageDecoder.OrientedPixelWidth * 0.5);
            imageEncoder.BitmapTransform.ScaledHeight = (uint)(imageDecoder.OrientedPixelHeight * 0.5);
            await imageEncoder.FlushAsync();
            return RandomAccessStreamReference.CreateFromStream(inMemoryStream);
        }

        private async Task<StorageFile> GetImage()
        {
            var imagePicker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter = { ".jpg", ".png" }
            };

            return await imagePicker.PickSingleFileAsync();
        }
    }
}
