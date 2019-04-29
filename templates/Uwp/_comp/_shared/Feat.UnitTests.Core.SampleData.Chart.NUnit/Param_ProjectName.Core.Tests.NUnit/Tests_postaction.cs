//{[{
using Param_RootNamespace.Core.Services;
using System.Threading.Tasks;
//}]}

namespace Param_RootNamespace.Core.Tests.NUnit
{
    public class Tests
    {
        //^^
        //{[{

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsChartDataAsync()
        {
            var actual = await SampleDataService.GetChartSampleDataAsync();

            Assert.IsNotEmpty(actual);
        }
        //}]}
    }
}
