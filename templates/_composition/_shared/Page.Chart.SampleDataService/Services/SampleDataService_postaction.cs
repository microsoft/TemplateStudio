using System.Collections.ObjectModel;
using System.Linq;

namespace Param_ItemNamespace.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO UWPTemplates: Remove this once your chart page is displaying real data
        public static ObservableCollection<DataPoint> GetChartSampleData()
        {
            var data = AllOrders().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
                                  .OrderBy(dp => dp.Category);

            return new ObservableCollection<DataPoint>(data);
        }
//}]}
    }
}
