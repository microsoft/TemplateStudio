using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Caliburn.Micro;
using Param_ItemNamespace.Helpers;
using Param_ItemNamespace.Models;

namespace Param_ItemNamespace.ViewModels
{
    public class wts.ItemNameExampleViewModel : Screen
    {
        private ShareOperation _shareOperation;

        private string _pageTitle;

        public string PageTitle
        {
            get => _pageTitle;
            set => Set(ref _pageTitle, value);
        }

        private SharedContentModel _sharedData;

        public SharedContentModel SharedData
        {
            get => _sharedData;
            set => Set(ref _sharedData, value);
        }

        public wts.ItemNameExampleViewModel()
        {
        }

        public async Task LoadAsync(ShareOperation shareOperation)
        {
            // ShareTarget declarations are defined in Package.appxmanifest.
            // Current declarations allow tho share WebLink and image files with the app.
            // ShareTarget can be tested sharing the WebLink from Microsoft Edge or sharing images from Windows Photos.
            _shareOperation = shareOperation;
            var newSharedData = new SharedContentModel
            {
                Title = _shareOperation.Data.Properties.Title
            };

            if (_shareOperation.Data.Contains(StandardDataFormats.WebLink))
            {
                PageTitle = "ShareTargetExample_WebLinkTitle".GetLocalized();
                newSharedData.DataFormat = StandardDataFormats.WebLink;
                newSharedData.Uri = await _shareOperation.GetWebLinkAsync();
            }

            if (_shareOperation.Data.Contains(StandardDataFormats.StorageItems))
            {
                PageTitle = "ShareTargetExample_ImagesTitle".GetLocalized();
                newSharedData.DataFormat = StandardDataFormats.StorageItems;
                var files = await _shareOperation.GetStorageItemsAsync();
                foreach (var file in files)
                {
                    var storageFile = file as StorageFile;
                    if (storageFile != null)
                    {
                        using (var inputStream = await storageFile.OpenReadAsync())
                        {
                            var img = new BitmapImage();
                            img.SetSource(inputStream);
                            newSharedData.Images.Add(img);
                        }
                    }
                }
            }

            SharedData = newSharedData;
        }

        public void Complete()
        {
            _shareOperation.ReportCompleted();
        }
    }
}
