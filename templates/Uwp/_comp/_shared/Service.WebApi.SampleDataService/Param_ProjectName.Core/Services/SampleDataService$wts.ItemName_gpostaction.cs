//{**
// This code block adds the method `GetWebApiSampleDataAsync()` to the SampleDataService of your project.
//**}
//{[{
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
//}]}

namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your Web API is returning real data.
        public static async Task<ObservableCollection<DataPoint>> GetWebApiSampleDataAsync()
        {
            await Task.CompletedTask;

            return new ObservableCollection<SampleOrder>(AllOrders());
        }
//}]}
    }
}
