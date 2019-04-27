using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.Views
{
    public sealed partial class ChartViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private ObservableCollection<DataPoint> _source;

        // TODO WTS: Change the chart as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        public ChartViewPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<DataPoint> Source
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
            Source = await SampleDataService.GetChartSampleDataAsync();
        }
    }
}
