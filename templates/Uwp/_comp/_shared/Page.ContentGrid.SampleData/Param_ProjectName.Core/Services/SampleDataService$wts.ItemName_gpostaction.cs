//{**
// This code block adds the method `GetContentGridDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//{[{
        private static IEnumerable<SampleOrder> _allOrders;
//}]}

        private static IEnumerable<SampleOrder> AllOrders()
        {
        }
//^^
//{[{

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        public static async Task<ObservableCollection<SampleOrder>> GetContentGridDataAsync()
        {
            await Task.CompletedTask;
            if (_allOrders == null)
            {
                _allOrders = AllOrders();
            }

            return new ObservableCollection<SampleOrder>(_allOrders);
        }
//}]}
    }
}
