using System.Collections.ObjectModel;

namespace Param_ItemNamespace.Services
{
    public static class SampleDataService
    {
//^^
//{[{
        // TODO UWPTemplates: Remove this once your chart page is displaying real data
        public static ObservableCollection<DataPoint> GetChartSampleData()
        {
            // The following is sales data
            var data = new ObservableCollection<DataPoint>
            {
                new DataPoint { Category = "January", Value = 3836.00 },
                new DataPoint { Category = "February", Value = 2241.50 },
                new DataPoint { Category = "March", Value = 16915.00 },
                new DataPoint { Category = "April", Value = 18975.25 },
                new DataPoint { Category = "May", Value = 1788.50 },
                new DataPoint { Category = "June", Value = 8306.50 },
                new DataPoint { Category = "July", Value = 8682.10 },
                new DataPoint { Category = "August", Value = 7719.00 },
                new DataPoint { Category = "September", Value = 12517.20 },
                new DataPoint { Category = "October", Value = 11663.75 },
                new DataPoint { Category = "November", Value = 10984.50 },
                new DataPoint { Category = "December", Value = 14273.60 },
            };

            return data;
        }
//}]}

    }
}
