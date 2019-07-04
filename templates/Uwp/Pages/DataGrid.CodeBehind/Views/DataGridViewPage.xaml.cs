using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.Views
{
    public sealed partial class DataGridViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private ObservableCollection<SampleOrder> _source;

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on DataGridViewPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public DataGridViewPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                 return _source;
            }

            set
            {
                Set(ref _source, value);
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // TODO WTS: Replace this with your actual data
            Source = await SampleDataService.GetGridDataAsync();
        }
    }
}
