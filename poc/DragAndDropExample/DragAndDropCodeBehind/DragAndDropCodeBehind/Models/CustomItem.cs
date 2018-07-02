using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace DragAndDropCodeBehind.Models
{
    public class CustomItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Path { get; set; }
        public string FileName { get; set; }
        public BitmapImage Image { get; set; }
        public IStorageItem OriginalStorageItem { get; set; }
    }

    public static class CustomItemFactory
    {
        public static async Task<CustomItem> Create(StorageFile item)
        {
            return new CustomItem
            {
                Path = item.Path,
                FileName = item.Name,
                Image = await GetImageOrDefaultAsync(item),
                OriginalStorageItem = item
            };
        }

        private static async Task<BitmapImage> GetImageOrDefaultAsync(StorageFile item)
        {
            try
            {
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(await item.OpenReadAsync());
                return bitmapImage;
            }
            catch (Exception)
            {
                return new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.png"));
            }
        }
    }
}
