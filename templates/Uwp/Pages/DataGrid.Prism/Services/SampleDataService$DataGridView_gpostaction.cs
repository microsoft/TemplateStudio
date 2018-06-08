namespace Param_ItemNamespace.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your grid page is displaying real data
        public ObservableCollection<SampleOrder> GetGridSampleData()
        {
            return new ObservableCollection<SampleOrder>(AllOrders());
        }
//}]}
    }
}
