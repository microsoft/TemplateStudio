using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

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

        public ObservableCollection<CustomPoint> Source
        {
            get
            {
                var collection = new ObservableCollection<CustomPoint>();
                collection.Add(new CustomPoint { Category = "Fred", Value = 6 });
                collection.Add(new CustomPoint { Category = "Hannah", Value = 18 });
                collection.Add(new CustomPoint { Category = "Steve", Value = 3 });
                collection.Add(new CustomPoint { Category = "Becky", Value = 9 });
                return collection;
            }
        }

        public class CustomPoint
        {
            public double Value { get; set; }
            public string Category { get; set; }
        }
    }
}
