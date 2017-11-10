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
    public class Scenario3ViewModel : Observable
    {
        public ObservableCollection<CustomItem> PrimaryItems { get; set; } = new ObservableCollection<CustomItem>();
        public ObservableCollection<CustomItem> SecondaryItems { get; set; } = new ObservableCollection<CustomItem>();


        private ICommand _getPrimaryItemsCommand;
        private ICommand _getSecondaryItemsCommand;
        private ICommand _getPrimaryIdsCommand;
        private ICommand _getSecondaryIdsCommand;
        private ICommand _dragItemStartingCommand;

        public ICommand GetPrimaryItemsCommand => _getPrimaryItemsCommand ?? (_getPrimaryItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetPrimaryItems));
        public ICommand GetSecondaryItemsCommand => _getSecondaryItemsCommand ?? (_getSecondaryItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetSecondaryItems));
        public ICommand GetPrimaryIdsCommand => _getPrimaryIdsCommand ?? (_getPrimaryIdsCommand = new RelayCommand<string>(OnGetPrimaryIds));
        public ICommand GetSecondaryIdsCommand => _getSecondaryIdsCommand ?? (_getSecondaryIdsCommand = new RelayCommand<string>(OnGetSecondaryIds));
        public ICommand DragItemStartingCommand => _dragItemStartingCommand ?? (_dragItemStartingCommand = new RelayCommand<DragDropStartingData>(OnDragItemStarting));

        public Scenario3ViewModel()
        {
        }

        private async void OnGetPrimaryItems(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                PrimaryItems.Add(await CustomItemFactory.Create(item));
            }
        }

        private async void OnGetSecondaryItems(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                SecondaryItems.Add(await CustomItemFactory.Create(item));
            }
        }
        
        private void OnGetPrimaryIds(string itemsId)
        {
            MoveItems(itemsId, SecondaryItems, PrimaryItems);
        }

        private void OnGetSecondaryIds(string itemsId)
        {
            MoveItems(itemsId, PrimaryItems, SecondaryItems);
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

        private void OnDragItemStarting(DragDropStartingData startingData)
        {
            var items = string.Join(",", startingData.Items.Cast<CustomItem>().Select(i => i.Id));
            startingData.Data.SetText(items);
            startingData.Data.RequestedOperation = DataPackageOperation.Move;
        }
    }
}
