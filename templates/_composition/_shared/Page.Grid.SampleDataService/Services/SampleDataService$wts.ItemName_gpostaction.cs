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
