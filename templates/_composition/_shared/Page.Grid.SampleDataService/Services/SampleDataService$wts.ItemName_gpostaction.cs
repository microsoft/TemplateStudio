//{**
// This code block adds the method `GetGridSampleData()` to the SampleDataService of your project.
//**}

namespace Param_ItemNamespace.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO UWPTemplates: Remove this once your grid page is displaying real data
        public static ObservableCollection<Order> GetGridSampleData()
        {
            return new ObservableCollection<Order>(AllOrders());
        }
//}]}
    }
}
