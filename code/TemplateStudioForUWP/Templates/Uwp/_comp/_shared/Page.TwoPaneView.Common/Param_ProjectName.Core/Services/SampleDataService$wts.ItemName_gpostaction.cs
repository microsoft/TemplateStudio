//{**
// This code block adds the method `GetTwoPaneViewDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // Remove this once your TwoPaneView pages are displaying real data.
        public static async Task<IEnumerable<SampleOrder>> GetTwoPaneViewDataAsync()
        {
            await Task.CompletedTask;
            return AllOrders();
        }
//}]}
    }
}
