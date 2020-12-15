namespace Param_RootNamespace.Core.Tests.MSTest
{
    public class SampleDataServiceTests
    {
        //^^
        //{[{

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [TestMethod]
        public async Task EnsureSampleDataServiceReturnsContentGridDataAsync()
        {
            var sampleDataService = new SampleDataService();

            var data = await sampleDataService.GetContentGridDataAsync();

            Assert.IsTrue(data.Any());
        }
        //}]}
    }
}
