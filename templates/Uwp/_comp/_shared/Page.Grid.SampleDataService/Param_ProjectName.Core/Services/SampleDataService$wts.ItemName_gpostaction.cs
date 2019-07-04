//{**
// This code block adds the method `GetGridDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your grid page is displaying real data.
        public static async Task<ObservableCollection<SampleOrder>> GetGridDataAsync()
        {
            await Task.CompletedTask;
            return new ObservableCollection<SampleOrder>(AllOrders());
        }
//}]}
    }
}
