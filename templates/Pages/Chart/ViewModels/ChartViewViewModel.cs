using System;
using System.Collections.ObjectModel;

namespace Param_ItemNamespace.ViewModels
{
    public class ChartViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public ChartViewViewModel()
        {
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
    }

    public class CustomPoint
    {
        public double Value { get; set; }
        public string Category { get; set; }
    }
}
