//{**
// This code block adds the method `GetGridSampleDataAsync()` to the SampleDataService of your project.
//**}
﻿//{[{
using System.Threading.Tasks;
//}]}
namespace Param_RootNamespace.Core.Services
{
    public static class SampleDataService
    {
//^^
//{[{

        // TODO WTS: Remove this once your grid page is displaying real data.
        public static async Task<ObservableCollection<SampleOrder>> GetGridSampleDataAsync()
        {
            var allOrders = await GetAllOrdersAsync();
            return new ObservableCollection<SampleOrder>(allOrders);
        }
//}]}
    }
}
