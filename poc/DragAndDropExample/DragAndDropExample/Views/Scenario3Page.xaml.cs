using System;
using System.Linq;
using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;
using DragAndDropExample.Models;
using Windows.ApplicationModel.DataTransfer;

namespace DragAndDropExample.Views
{
    public sealed partial class Scenario3Page : Page
    {
        public Scenario3ViewModel ViewModel { get; } = new Scenario3ViewModel();

        public Scenario3Page()
        {
            InitializeComponent();
        }

        //This is listview event, cannot use in uielement service
        private void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var items = string.Join(",", e.Items.Cast<CustomItem>().Select(i => i.Id));
            e.Data.SetText(items);
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }
    }
}
