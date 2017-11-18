using DragAndDropCodeBehind.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace DragAndDropCodeBehind.Views
{
    public sealed partial class Scenario3Page : Page, INotifyPropertyChanged
    {
        public ObservableCollection<CustomItem> PrimaryItems { get; set; } = new ObservableCollection<CustomItem>();
        public ObservableCollection<CustomItem> SecondaryItems { get; set; } = new ObservableCollection<CustomItem>();

        public Action<IReadOnlyList<IStorageItem>> GetPrimaryItems => async (items) => await OnGetPrimaryItems(items);
        public Action<IReadOnlyList<IStorageItem>> GetSecondaryItems => async (items) => await OnGetSecondaryItems(items);
        public Action<string> GetPrimaryIds => (itemsId) => OnGetPrimaryIds(itemsId);
        public Action<string> GetSecondaryIds => (itemsId) => OnGetSecondaryIds(itemsId);
        public Action<DragDropStartingData> DragItemStarting => (startingData) => OnDragItemStarting(startingData);
        public Action<DragDropData> ListDragOver => (overData) => OnDragOver(overData);

        public Scenario3Page()
        {
            InitializeComponent();
        }
        
        private async Task OnGetPrimaryItems(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                PrimaryItems.Add(await CustomItemFactory.Create(item));
            }
        }

        private async Task OnGetSecondaryItems(IReadOnlyList<IStorageItem> items)
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

        private void OnDragOver(DragDropData overData)
        {
            overData.AcceptedOperation = overData.DataView.Contains(StandardDataFormats.Text)
                ? DataPackageOperation.Move
                : DataPackageOperation.Copy;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
