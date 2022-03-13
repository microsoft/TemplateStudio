using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using Param_RootNamespace.Helpers;

namespace Param_RootNamespace.ViewModels
{
    public class SharedDataStorageItemsViewModel : SharedDataViewModelBase
    {
        public ObservableCollection<ImageSource> Images { get; } = new ObservableCollection<ImageSource>();

        public SharedDataStorageItemsViewModel()
        {
        }

        public override async Task LoadDataAsync(ShareOperation shareOperation)
        {
            await base.LoadDataAsync(shareOperation);

            PageTitle = "ShareTargetFeature_ImagesTitle".GetLocalized();
            DataFormat = StandardDataFormats.StorageItems;
            var files = await shareOperation.GetStorageItemsAsync();
            foreach (var file in files)
            {
                var storageFile = file as StorageFile;
                if (storageFile != null)
                {
                    using (var inputStream = await storageFile.OpenReadAsync())
                    {
                        var img = new BitmapImage();
                        img.SetSource(inputStream);
                        Images.Add(img);
                    }
                }
            }
        }
    }
}
