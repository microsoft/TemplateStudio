using DragAndDropCodeBehind.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace DragAndDropCodeBehind.Views
{
    public sealed partial class Scenario1Page : Page, INotifyPropertyChanged
    {
        public ObservableCollection<CustomItem> Items { get; private set; } = new ObservableCollection<CustomItem>();

        public Action<IReadOnlyList<IStorageItem>> GetStorageItem => async (items) => await OnGetStorageItem(items);

        public Scenario1Page()
        {
            InitializeComponent();
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

        public async Task OnGetStorageItem(IReadOnlyList<IStorageItem> items)
        {
            foreach (StorageFile item in items)
            {
                Items.Add(await CustomItemFactory.Create(item));
            }
        }
    }
}
