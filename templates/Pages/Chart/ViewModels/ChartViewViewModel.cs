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
                collection.Add(new CustomPoint { Label = "Fred", Value = 6 });
                collection.Add(new CustomPoint { Label = "Hannah", Value = 18 });
                collection.Add(new CustomPoint { Label = "Steve", Value = 3 });
                collection.Add(new CustomPoint { Label = "Becky", Value = 9 });
                return collection;
            }
        }
    }

    public class CustomPoint
    {
        public double Value { get; set; }
        public string Label { get; set; }
    }
}