//{**
// This code block adds the method `GetGridSampleDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        private static IEnumerable<SampleOrder> _allOrders;

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        public static async Task<ObservableCollection<SampleOrder>> GetContentGridDataAsync()
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
