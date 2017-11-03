using System;

using DragAndDropExample.Helpers;
using System.Collections.ObjectModel;
using DragAndDropExample.Models;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

namespace DragAndDropExample.ViewModels
{
    public class Scenario5ViewModel : Observable
    {
        public ObservableCollection<CustomItem> Items { get; set; } = new ObservableCollection<CustomItem>();
        private ICommand _getStorageItemsCommand;

        public ICommand GetStorageItemsCommand => _getStorageItemsCommand ?? (_getStorageItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetStorageItem));


        public Scenario5ViewModel()
        {
        }

        private async void OnGetStorageItem(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                Items.Add(new CustomItem
                {
                    Path = item.Path,
                    FileName = item.Name,
                    Image = await GetImageOrDefaultAsync(item),
                    OriginalStorageItem = item
                });
            }
        }

        private async Task<BitmapImage> GetImageOrDefaultAsync(StorageFile item)
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
