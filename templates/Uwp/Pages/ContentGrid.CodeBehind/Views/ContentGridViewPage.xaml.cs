using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.Core.Services;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ContentGridViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetContentGridData();
            }
        }

        public ContentGridViewPage()
        {
            InitializeComponent();
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is SampleOrder item)
            {
                NavigationService.Navigate<ContentGridViewDetailPage>(item);
            }
        }
    }
}
