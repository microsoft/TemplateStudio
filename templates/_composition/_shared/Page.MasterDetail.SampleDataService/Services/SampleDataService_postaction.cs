using System.Threading.Tasks;
namespace Param_ItemNamespace.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO UWPTemplates: Remove this once your MasterDetail pages are displaying real data
        public static async Task<IEnumerable<Order>> GetSampleModelDataAsync()
        {
            await Task.CompletedTask;

            return AllOrders();
        }
//}]}
    }
}
