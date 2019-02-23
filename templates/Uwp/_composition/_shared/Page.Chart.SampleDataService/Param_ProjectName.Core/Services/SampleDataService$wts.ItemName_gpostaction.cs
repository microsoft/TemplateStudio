//{**
// This code block adds the method `GetChartSampleData()` to the SampleDataService of your project.
//**}
//{[{
using System.Collections.ObjectModel;
using System.Linq;
//}]}

namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your chart page is displaying real data.
        public static ObservableCollection<DataPoint> GetChartSampleData()
        {
            var data = AllOrders().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
                                  .OrderBy(dp => dp.Category);

            return new ObservableCollection<DataPoint>(data);
        }
//}]}
    }
}
