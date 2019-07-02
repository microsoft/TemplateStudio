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
//{[{
        private IEnumerable<SampleOrder> _allOrders;
//}]}

        private IEnumerable<SampleOrder> AllOrders()
        {
        }
//^^
//{[{

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        public async Task<ObservableCollection<SampleOrder>> GetContentGridDataAsync()
        {
            if (_allOrders == null)
            {
                _allOrders = AllOrders();
            }

            await Task.CompletedTask;
            return new ObservableCollection<SampleOrder>(_allOrders);
        }
//}]}
    }
}
