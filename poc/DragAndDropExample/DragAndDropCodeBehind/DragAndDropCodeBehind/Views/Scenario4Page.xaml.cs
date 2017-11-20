using DragAndDropCodeBehind.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DragAndDropCodeBehind.Views
{
    public sealed partial class Scenario4Page : Page, INotifyPropertyChanged
    {
        public ObservableCollection<CustomItem> Items { get; private set; } = new ObservableCollection<CustomItem>();

        
        public Action<IReadOnlyList<IStorageItem>> GetStorageItems => async (items) => await OnGetStorageItems(items);
        public Action<DragDropData> DragEnterAction => (items) => OnDragEnter(items);
        public Action<DragDropData> DragLeaveAction => (items) => OnDragLeave(items);

        public Scenario4Page()
        {
            InitializeComponent();
        }

        private async Task OnGetStorageItems(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                Items.Add(await CustomItemFactory.Create(item));
            }

            DragMask.Visibility = Visibility.Collapsed;
        }

        private void OnDragEnter(DragDropData overData)
        {
            DragMask.Visibility = Visibility.Visible;
        }

        private void OnDragLeave(DragDropData overData)
        {
            DragMask.Visibility = Visibility.Collapsed;
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
