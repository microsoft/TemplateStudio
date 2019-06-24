//{**
// This code block adds the method `GetChartSampleDataAsync()` to the SampleDataService of your project.
//**}
//{[{
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
//}]}

namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your chart page is displaying real data.
        public static async Task<ObservableCollection<DataPoint>> GetChartSampleDataAsync()
        {
            var allOrders = await GetAllOrdersAsync();
            var data = allOrders.Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
                                  .OrderBy(dp => dp.Category);

            return new ObservableCollection<DataPoint>(data);
        }
//}]}
    }
}
