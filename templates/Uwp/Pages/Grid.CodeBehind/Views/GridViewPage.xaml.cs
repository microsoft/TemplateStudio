using System;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.Core.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class GridViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on GridViewPage.xaml.
        // For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        // You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
        public GridViewPage()
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
