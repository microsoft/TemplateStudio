//{**
// This code block adds the method `GetChartSampleData()` to the SampleDataService of your project.
//**}
//{[{
using System.Collections.ObjectModel;
using System.Linq;
//}]}

namespace Param_ItemNamespace.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // TODO WTS: Delete this file once your app is using real data.
//{--{public static class SampleDataService//}--}
//{[{
    public class SampleDataService : ISampleDataService
//}]}
    {
//^^
//{[{

        // TODO WTS: Remove this once your chart page is displaying real data
        public ObservableCollection<DataPoint> GetChartSampleData()
        {
            var data = AllOrders().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
                                  .OrderBy(dp => dp.Category);

            return new ObservableCollection<DataPoint>(data);
        }
//}]}
    }
}
