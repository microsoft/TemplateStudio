//{**
// This code block adds the method `GetGridSampleDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your TreeView page is displaying real data.
        public static async Task<IEnumerable<SampleCompany>> GetCompaniesDataAsync()
        {
            return await GetDataAsync();
        }
//}]}
    }
}
