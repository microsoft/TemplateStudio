//{**
// This code block adds the method `GetTreeViewDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your TreeView page is displaying real data.
        public async Task<IEnumerable<SampleCompany>> GetTreeViewDataAsync()
        {
            await Task.CompletedTask;
            return AllCompanies();
        }
//}]}
    }
}
