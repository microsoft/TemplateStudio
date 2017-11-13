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
    public sealed partial class Scenario2Page : Page, INotifyPropertyChanged
    {
        public ObservableCollection<CustomItem> Items { get; set; } = new ObservableCollection<CustomItem>();

        public Action<IReadOnlyList<IStorageItem>> GetStorageItems => async (items) => await OnGetStorageItems(items);
        public Action<DragDropStartingData> DragItemStarting => (startingData) => OnDragItemStarting(startingData);
        public Action<DragDropCompletedData> DragItemCompleted => (completedData) => OnDragItemCompleted(completedData);

        public Scenario2Page()
        {
            InitializeComponent();
        }


        private async Task OnGetStorageItems(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                Items.Add(await CustomItemFactory.Create(item));
            }
            listview.AllowDrop = !Items.Any();
        }

        private void OnDragItemStarting(DragDropStartingData startingData)
        {
            var items = startingData.Items.Cast<CustomItem>().Select(i => i.OriginalStorageItem);
            startingData.Data.SetStorageItems(items);
            startingData.Data.RequestedOperation = DataPackageOperation.Copy;
        }

        private void OnDragItemCompleted(DragDropCompletedData completedData)
        {
            if (completedData.DropResult != DataPackageOperation.None)
            {
                var draggedItems = completedData.Items.Cast<CustomItem>();
                foreach (var item in draggedItems)
                {
                    Items.Remove(item);
                }

                listview.AllowDrop = !Items.Any();
            }
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
