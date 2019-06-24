//{[{
using System.Threading.Tasks;
//}]}
namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your grid page is displaying real data.
        public async Task<ObservableCollection<SampleOrder>> GetGridSampleDataAsync()
        {
            var allOrders = await GetAllOrdersAsync();
            return new ObservableCollection<SampleOrder>(allOrders);
        }
//}]}
    }
}
