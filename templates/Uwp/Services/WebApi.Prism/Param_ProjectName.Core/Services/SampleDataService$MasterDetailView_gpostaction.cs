//{**
// This code block adds the method `GetWebApiSampleDataAsync()` to the SampleDataService of your project.
//**}
namespace Param_RootNamespace.Core.Services
{
    public class SampleDataService : ISampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your Web API is returning real data.
        public async Task<IEnumerable<SampleCompany>> GetWebApiSampleDataAsync()
        {
            await Task.CompletedTask;

            return AllCompanies();
        }
//}]}
    }
}
