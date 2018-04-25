using System;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class DataGridViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO WTS: Change the grid as appropriate to your app.
        // For help see XXXXXXXXX
        public DataGridViewPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
