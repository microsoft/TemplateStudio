namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your MasterDetail pages are displaying real data.
        public async Task<IEnumerable<SampleOrder>> GetMasterDetailDataAsync()
        {
            await Task.CompletedTask;
            return AllOrders();
        }
//}]}
    }
}