using DragAndDropExample.Helpers;
using DragAndDropExample.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace DragAndDropExample.ViewModels
{
    public class Scenario2ViewModel : Observable
    {
        public ObservableCollection<CustomItem> Items { get; set; } = new ObservableCollection<CustomItem>();

        private ICommand _getStorageItemsCommand;
        private ICommand _dragItemStartingCommand;
        private ICommand _dragItemCompletedCommand;

        public ICommand GetStorageItemsCommand => _getStorageItemsCommand ?? (_getStorageItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetStorageItem));
        public ICommand DragItemStartingCommand => _dragItemStartingCommand ?? (_dragItemStartingCommand = new RelayCommand<DragDropStartingData>(OnDragItemStarting));
        public ICommand DragItemCompletedCommand => _dragItemCompletedCommand ?? (_dragItemCompletedCommand = new RelayCommand<DragDropCompletedData>(OnDragItemCompleted));

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
            AllowDrop = !Items.Any();
        }

        private void OnDragItemStarting(DragDropStartingData startingData)
        {
            var items = startingData.Items.Cast<CustomItem>().Select(i => i.OriginalStorageItem);
            startingData.Data.SetStorageItems(items);
            startingData.Data.RequestedOperation = DataPackageOperation.Copy;
        }

        private void OnDragItemCompleted(DragDropCompletedData completedData)
        {
            if(completedData.DropResult != DataPackageOperation.None)
            {
                var draggedItems = completedData.Items.Cast<CustomItem>();
                foreach(var item in draggedItems)
                {
                    Items.Remove(item);
                }

                AllowDrop = !Items.Any();
            }
        }

        private bool _allowDrop;
        public bool AllowDrop
        {
            get => _allowDrop;
            set => Set(ref _allowDrop, value);
        }
    }
}
