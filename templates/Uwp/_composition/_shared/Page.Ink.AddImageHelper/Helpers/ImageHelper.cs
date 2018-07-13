using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;

namespace Param_ItemNamespace.Helpers
{
    public static class ImageHelper
    {
        public static async Task<StorageFile> LoadImageFileAsync()
        {
            var openPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".bmp");

            var imageFile = await openPicker.PickSingleFileAsync();

            return imageFile;
        }

        public static async Task<BitmapImage> GetBitmapFromImageAsync(StorageFile file)
        {
            if (file == null)
            {
                return null;
            }

            try
            {
                using (var fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream);
                    return bitmapImage;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
