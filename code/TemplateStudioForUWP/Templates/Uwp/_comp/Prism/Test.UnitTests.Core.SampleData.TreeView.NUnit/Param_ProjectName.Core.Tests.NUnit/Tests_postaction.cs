//{[{
using System.Linq;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Services;
//}]}

namespace Param_RootNamespace.Core.Tests.NUnit
{
    public class Tests
    {
        //^^
        //{[{

        // Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsTreeViewDataAsync()
        {
            var dataService = new SampleDataService();
            var actual = await dataService.GetTreeViewDataAsync();

            Assert.AreNotEqual(0, actual.Count());
        }
        //}]}
    }
}
