using System;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class GridViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO UWPTemplates: Change the grid as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        // You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
        public GridViewPage()
        {
            InitializeComponent();
            
            // TODO: Moving from x:Bind to Binding as workaround for https://github.com/Microsoft/WindowsTemplateStudio/issues/496
            // Once Telerik fix the root cause, we get back to x:Bind and remove the following line
            DataContext = this;
        }

        public ObservableCollection<Order> Source
        {
            get
            {
                // TODO UWPTemplates: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
