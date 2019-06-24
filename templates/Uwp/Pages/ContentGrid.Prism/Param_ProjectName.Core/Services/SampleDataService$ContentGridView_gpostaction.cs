//{**
// This code block adds the method `GetContentGridDataAsync()` to the SampleDataService of your project.
//**}
//{[{
using System.Threading.Tasks;
//}]}
namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        private static IEnumerable<SampleOrder> _allOrders;

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        public async Task<ObservableCollection<SampleOrder>> GetContentGridDataAsync()
        {
            if (_allOrders == null)
            {
                _allOrders = await GetAllOrdersAsync();
            }

            return new ObservableCollection<SampleOrder>(_allOrders);
        }
//}]}
    }
}
