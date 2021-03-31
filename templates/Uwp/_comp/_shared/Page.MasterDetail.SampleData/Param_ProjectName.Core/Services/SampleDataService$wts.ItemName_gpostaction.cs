//{**
// This code block adds the method `GetListDetailDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your ListDetail pages are displaying real data.
        public static async Task<IEnumerable<SampleOrder>> GetListDetailDataAsync()
        {
            await Task.CompletedTask;
            return AllOrders();
        }
//}]}
    }
}
