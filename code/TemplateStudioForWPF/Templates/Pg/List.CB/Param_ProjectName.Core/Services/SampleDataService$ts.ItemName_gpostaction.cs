namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // Remove this once your ListDetails pages are displaying real data.
        public async Task<IEnumerable<SampleOrder>> GetListDetailsDataAsync()
        {
            await Task.CompletedTask;
            return AllOrders();
        }
//}]}
    }
}