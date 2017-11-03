using System;

using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.DataTransfer;
using DragAndDropExample.Models;
using System.Linq;
namespace DragAndDropExample.Views
{
    public sealed partial class Scenario5Page : Page
    {
        public Scenario5ViewModel ViewModel { get; } = new Scenario5ViewModel();

        public Scenario5Page()
        {
            InitializeComponent();
        }

        //this can implemented with event to command for mvvm pattern
        private async void ListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            var dataview = e.DataView;
            if (dataview.Contains(StandardDataFormats.StorageItems))
            {
                var storageItems = await dataview.GetStorageItemsAsync();
                ViewModel.GetStorageItemsCommand.Execute(storageItems);
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
    }
}
