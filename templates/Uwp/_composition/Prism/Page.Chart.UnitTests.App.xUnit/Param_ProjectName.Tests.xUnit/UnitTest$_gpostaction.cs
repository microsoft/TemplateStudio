//{[{
using Param_RootNamespace.ViewModels;
using Param_RootNamespace.Core.Services;
using Moq;
//}]}

namespace Param_RootNamespace.Tests.xUnit
{
    public class Tests
    {
        //^^
        //{[{

        // TODO WTS: Add tests for functionality you add to wts.ItemNameViewModel.
        [Fact]
        public void Testwts.ItemNameViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var mockDataService = new Mock<ISampleDataService>();
            var vm = new ChartViewModel(mockDataService.Object);
            Assert.NotNull(vm);
        }
        //}]}
    }
}
