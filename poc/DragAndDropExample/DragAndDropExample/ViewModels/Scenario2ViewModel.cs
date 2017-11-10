using System;

using DragAndDropExample.Helpers;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Storage;
using System.Collections.ObjectModel;
using System.Linq;
using DragAndDropExample.Models;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace DragAndDropExample.ViewModels
{
    public class Scenario2ViewModel : Observable
    {
        public ObservableCollection<CustomItem> Items { get; set; } = new ObservableCollection<CustomItem>();

        private ICommand _getStorageItemsCommand;
        private ICommand _dragItemStartingCommand;

        public ICommand GetStorageItemsCommand => _getStorageItemsCommand ?? (_getStorageItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetStorageItem));
        public ICommand DragItemStartingCommand => _dragItemStartingCommand ?? (_dragItemStartingCommand = new RelayCommand<DragDropStartingData>(OnDragItemStarting));

        public Scenario2ViewModel()
        {
            AllowDrop = true;
        }

        private async void OnGetStorageItem(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                Items.Add(await CustomItemFactory.Create(item));
            }

            //Disable allow drop
            //AllowDrop = false;
        }
        private void OnDragItemStarting(DragDropStartingData startingData)
        {
            var items = startingData.Items.Cast<CustomItem>().Select(i => i.OriginalStorageItem);
            startingData.Data.SetStorageItems(items);
            startingData.Data.RequestedOperation = DataPackageOperation.Move;
        }

        private bool _allowDrop;
        public bool AllowDrop
        {
            get => _allowDrop;
            set => Set(ref _allowDrop, value);
        }

    }
}
