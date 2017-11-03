using System;
using System.Linq;
using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;
using DragAndDropExample.Models;
using Windows.ApplicationModel.DataTransfer;

namespace DragAndDropExample.Views
{
    public sealed partial class Scenario2Page : Page
    {
        public Scenario2ViewModel ViewModel { get; } = new Scenario2ViewModel();

        public Scenario2Page()
        {
            InitializeComponent();
        }

        //This is listview event, cannot use in uielement service
        private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var items = e.Items.Cast<CustomItem>().Select(i => i.OriginalStorageItem);
            e.Data.SetStorageItems(items);
            e.Data.RequestedOperation = DataPackageOperation.Copy;
        }
    }
}
