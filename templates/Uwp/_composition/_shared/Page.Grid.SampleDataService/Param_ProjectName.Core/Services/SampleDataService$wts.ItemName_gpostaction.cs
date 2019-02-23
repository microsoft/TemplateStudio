//{**
// This code block adds the method `GetGridSampleData()` to the SampleDataService of your project.
//**}

namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your grid page is displaying real data.
        public static ObservableCollection<SampleOrder> GetGridSampleData()
        {
            return new ObservableCollection<SampleOrder>(AllOrders());
        }
//}]}
    }
}
