namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // Remove this once your DataGrid pages are displaying real data.
        public async Task<IEnumerable<SampleOrder>> GetGridDataAsync()
        {
            await Task.CompletedTask;
            return AllOrders();
        }
//}]}
    }
}