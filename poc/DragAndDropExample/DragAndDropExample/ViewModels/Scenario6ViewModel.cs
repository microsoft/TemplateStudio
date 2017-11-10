using System;
using System.Linq;
using DragAndDropExample.Helpers;
using System.Collections.ObjectModel;
using DragAndDropExample.Models;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace DragAndDropExample.ViewModels
{
    public class Scenario6ViewModel : Observable
    {
        public ObservableCollection<CustomItem> PrimaryItems { get; set; } = new ObservableCollection<CustomItem>();
        public ObservableCollection<CustomItem> SecondaryItems { get; set; } = new ObservableCollection<CustomItem>();
        private ICommand _getPrimaryItemsCommand;
        private ICommand _getSecondaryItemsCommand;
        public ICommand GetPrimaryItemsCommand => _getPrimaryItemsCommand ?? (_getPrimaryItemsCommand = new RelayCommand<DataPackageView>(OnGetPrimaryItems));
        public ICommand GetSecondaryItemsCommand => _getSecondaryItemsCommand ?? (_getSecondaryItemsCommand = new RelayCommand<DataPackageView>(OnGetSecondaryItems));

        public Scenario6ViewModel()
        {
        }


        private async void OnGetPrimaryItems(DataPackageView dataview)
        {
            if (dataview.Contains(StandardDataFormats.StorageItems))
            {
                await InsertItems(dataview, PrimaryItems);
            }

            if (dataview.Contains(StandardDataFormats.Text))
            {
                var itemsId = await dataview.GetTextAsync();
                MoveItems(itemsId, SecondaryItems, PrimaryItems);
            }
        }

        private async void OnGetSecondaryItems(DataPackageView dataview)
        {
            if (dataview.Contains(StandardDataFormats.StorageItems))
            {
                await InsertItems(dataview, SecondaryItems);
            }
            if (dataview.Contains(StandardDataFormats.Text))
            {
                var itemsId = await dataview.GetTextAsync();
                MoveItems(itemsId, PrimaryItems, SecondaryItems);
            }
        }

        private async Task InsertItems(DataPackageView dataview, ObservableCollection<CustomItem> target)
        {
            if (dataview.Contains(StandardDataFormats.StorageItems))
            {
                var items = await dataview.GetStorageItemsAsync();
                foreach (StorageFile item in items)
                {
                    target.Add(await CustomItemFactory.Create(item));
                }
            }
        }

        private void MoveItems(string itemsId, ObservableCollection<CustomItem> source, ObservableCollection<CustomItem> target)
        {
            var itemIdsToMove = itemsId.Split(',');
            foreach (var id in itemIdsToMove)
            {
                var item = source.FirstOrDefault(i => i.Id.ToString() == id);
                if (item != null)
                {
                    source.Remove(item);
                    target.Add(item);
                }
            }
        }
    }
}
