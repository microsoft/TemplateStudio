using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.Views
{
    public sealed partial class ChartViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO: UWPTemplates: Change the chart as appropriate to your app.
        // For help see http://docs.telerik.com/windows-universal/controls/radchart/getting-started
        public ChartViewPage()
        {
            InitializeComponent();
        }

        public ObservableCollection<SampleDataService.DataPoint> Source
        {
            get
            {
                // TODO UWPTemplates: Replace this with your actual data
                return SampleDataService.GetChartSampleData();
            }
        }
    }
}
