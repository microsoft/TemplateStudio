using DragAndDropExample.Models;
using DragAndDropExample.ViewModels;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;

namespace DragAndDropExample.Views
{
    public sealed partial class Scenario6Page : Page
    {
        public Scenario6ViewModel ViewModel { get; } = new Scenario6ViewModel();

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

        //this can implemented with event to command for mvvm pattern
        private void PrimaryListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            ViewModel.GetPrimaryItemsCommand.Execute(e.DataView);
        }

        //this can implemented with event to command for mvvm pattern
        private void SecondaryListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            ViewModel.GetSecondaryItemsCommand.Execute(e.DataView);
        }


    }
}
