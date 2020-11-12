namespace Param_RootNamespace.Core.Tests.XUnit
{
    public class SampleDataServiceTests
    {
        //^^
        //{[{

        // TODO WTS: Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Fact]
        public async Task EnsureSampleDataServiceReturnsMasterDetailDataAsync()
        {
            var sampleDataService = new SampleDataService();

            var data = await sampleDataService.GetMasterDetailDataAsync();

            Assert.True(data.Any());
        }
        //}]}
    }
}
