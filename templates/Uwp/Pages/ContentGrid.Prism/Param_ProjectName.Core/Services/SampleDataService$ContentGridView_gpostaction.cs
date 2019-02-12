//{**
// This code block adds the method `GetContentGridData()` to the SampleDataService of your project.
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
        public ObservableCollection<SampleOrder> GetContentGridData()
        {
            if (_allOrders == null)
            {
                _allOrders = AllOrders();
            }

            return new ObservableCollection<SampleOrder>(_allOrders);
        }
//}]}
    }
}
