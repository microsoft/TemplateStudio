//{[{
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;
//}]}
namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your chart page is displaying real data.
        public async Task<ObservableCollection<DataPoint>> GetChartSampleDataAsync()
        {
            var allOrders = await GetAllOrdersAsync();
            var data = allOrders.Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
                                  .OrderBy(dp => dp.Category);

            return new ObservableCollection<DataPoint>(data);
        }
//}]}
    }
}
