using DragAndDropCodeBehind.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace DragAndDropCodeBehind.Views
{
    public sealed partial class Scenario5Page : Page, INotifyPropertyChanged
    {
        public ObservableCollection<CustomItem> Items { get; private set; } = new ObservableCollection<CustomItem>();

        public Scenario5Page()
        {
            InitializeComponent();
        }

        private async void ListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            var dataview = e.DataView;
            if (dataview.Contains(StandardDataFormats.StorageItems))
            {
                var storageItems = await dataview.GetStorageItemsAsync();
                foreach (StorageFile item in storageItems)
                {
                    Items.Add(await CustomItemFactory.Create(item));
                }
            }
        }

        private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var items = e.Items.Cast<CustomItem>().Select(i => i.OriginalStorageItem);
            e.Data.SetStorageItems(items);
            e.Data.RequestedOperation = DataPackageOperation.Copy;
        }

        private void ListView_DragOver(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
                e.AcceptedOperation = DataPackageOperation.Copy;
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
