//{**
// This code block adds the method `GetGridSampleData()` to the SampleDataService of your project.
//**}

namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        private static IEnumerable<SampleOrder> _allOrders;

        // TODO WTS: Remove this once your ContentGrid page is displaying real data.
        public static ObservableCollection<SampleOrder> GetContentGridData()
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
