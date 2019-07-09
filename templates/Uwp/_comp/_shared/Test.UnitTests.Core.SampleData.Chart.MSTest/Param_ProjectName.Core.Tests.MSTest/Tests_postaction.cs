//{[{
using Param_RootNamespace.Core.Services;
//}]}

namespace Param_RootNamespace.Core.Tests.MSTest
{
    public class Tests
    {
        //^^
        //{[{

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [TestMethod]
        public async void EnsureSampleDataServiceReturnsChartDataAsync()
        {
            var actual = await SampleDataService.GetChartDataAsync();

            Assert.AreNotEqual(0, actual.Count());
        }
        //}]}
    }
}
