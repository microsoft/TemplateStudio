//{**
// This code block adds the method `GetGridDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your grid page is displaying real data.
        public async Task<IEnumerable<SampleOrder>> GetGridDataAsync()
        {
            await Task.CompletedTask;
            return AllOrders();
        }
//}]}
    }
}
