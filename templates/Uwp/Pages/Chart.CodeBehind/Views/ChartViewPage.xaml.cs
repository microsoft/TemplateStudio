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
        public ObservableCollection<DataPoint> Source { get; } = new ObservableCollection<DataPoint>();

        // TODO WTS: Change the chart as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        public ChartViewPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await SampleDataService.GetChartDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
