using DragAndDropCodeBehind.Models;
using System;
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
    public sealed partial class Scenario6Page : Page, INotifyPropertyChanged
    {
        public ObservableCollection<CustomItem> PrimaryItems { get; private set; } = new ObservableCollection<CustomItem>();
        public ObservableCollection<CustomItem> SecondaryItems { get; private set; } = new ObservableCollection<CustomItem>();

        public Scenario6Page()
        {
            InitializeComponent();
        }

        private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var items = string.Join(",", e.Items.Cast<CustomItem>().Select(i => i.Id));
            e.Data.SetText(items);
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        private void ListView_DragOver(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
                e.AcceptedOperation = DataPackageOperation.Copy;

            if (e.DataView.Contains(StandardDataFormats.Text))
                e.AcceptedOperation = DataPackageOperation.Move;
        }
        
        private async void PrimaryListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            await ExecuteDrop(e.DataView, SecondaryItems, PrimaryItems);
        }
        
        private async void SecondaryListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            await ExecuteDrop(e.DataView, PrimaryItems, SecondaryItems);
        }

        private async Task ExecuteDrop(DataPackageView dataview, ObservableCollection<CustomItem> source, ObservableCollection<CustomItem> target)
        {
            if (dataview.Contains(StandardDataFormats.StorageItems))
            {
                await InsertItems(dataview, target);
            }

            if (dataview.Contains(StandardDataFormats.Text))
            {
                var itemsId = await dataview.GetTextAsync();
                MoveItems(itemsId, source, target);
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
