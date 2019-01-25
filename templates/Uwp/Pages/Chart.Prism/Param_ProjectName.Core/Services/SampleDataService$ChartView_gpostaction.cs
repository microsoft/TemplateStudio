//{[{
using Param_ItemNamespace.Core.Models;
//}]}
namespace Param_ItemNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your chart page is displaying real data.
        public ObservableCollection<DataPoint> GetChartSampleData()
        {
            var data = AllOrders().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
                                  .OrderBy(dp => dp.Category);

            return new ObservableCollection<DataPoint>(data);
        }
//}]}
    }
}
