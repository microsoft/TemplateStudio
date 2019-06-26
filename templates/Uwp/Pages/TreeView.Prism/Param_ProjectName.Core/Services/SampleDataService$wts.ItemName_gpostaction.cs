//{[{
using System.Threading.Tasks;
//}]}

namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your TreeView page is displaying real data.
        public async Task<IEnumerable<SampleCompany>> GetTreeViewDataAsync()
        {
            return await GetDataAsync();
        }
//}]}
    }
}
