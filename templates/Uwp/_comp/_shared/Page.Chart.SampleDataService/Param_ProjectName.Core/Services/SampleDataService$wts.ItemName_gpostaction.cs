//{**
// This code block adds the method `GetChartDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your chart page is displaying real data.
        public static async Task<IEnumerable<DataPoint>> GetChartDataAsync()
        {
            await Task.CompletedTask;
            return AllOrders().Select(o => new DataPoint() { Category = o.Company, Value = o.OrderTotal })
                                  .OrderBy(dp => dp.Category);
        }
//}]}
    }
}
