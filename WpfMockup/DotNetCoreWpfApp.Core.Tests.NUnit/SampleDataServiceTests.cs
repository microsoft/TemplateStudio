using System.Linq;
using System.Threading.Tasks;
using DotNetCoreWpfApp.Core.Services;
using NUnit.Framework;

namespace DotNetCoreWpfApp.Core.Tests.NUnit
{
    public class SampleDataServiceTests
    {
        public SampleDataServiceTests()
        {

        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsContentGridDataAsync()
        {
            var sampleDataService = new SampleDataService();

            var data = await sampleDataService.GetContentGridDataAsync();

            Assert.IsTrue(data.Any());
        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsGridDataAsync()
        {
            var sampleDataService = new SampleDataService();

            var data = await sampleDataService.GetGridDataAsync();

            Assert.IsTrue(data.Any());
        }

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Test]
        public async Task EnsureSampleDataServiceReturnsMasterDetailDataAsync()
        {
            var sampleDataService = new SampleDataService();

            var data = await sampleDataService.GetMasterDetailDataAsync();

            Assert.IsTrue(data.Any());
        }
    }
}
