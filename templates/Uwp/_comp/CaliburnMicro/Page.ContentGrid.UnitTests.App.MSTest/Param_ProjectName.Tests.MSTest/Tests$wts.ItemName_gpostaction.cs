//{[{
using Param_RootNamespace.ViewModels;
using Param_RootNamespace.Services;
using Caliburn.Micro;
using Moq;
//}]}

namespace Param_RootNamespace.Tests.MSTest
{
    public class Tests
    {
        //^^
        //{[{

        // TODO WTS: Add tests for functionality you add to wts.ItemNameViewModel.
        [TestMethod]
        public void Testwts.ItemNameViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var mockNavService = new Mock<INavigationService>();
            var mockAnimationService = new Mock<IConnectedAnimationService>();
            var vm = new wts.ItemNameViewModel(mockNavService.Object, mockAnimationService.Object);
            Assert.IsNotNull(vm);
        }
        //}]}
    }
}
