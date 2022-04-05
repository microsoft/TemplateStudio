//{[{
using Param_RootNamespace.ViewModels;
//}]}

namespace Param_RootNamespace.Tests.XUnit
{
    public class Tests
    {
        //^^
        //{[{

        // TODO: Add tests for functionality you add to wts.ItemNameViewModel.
        [Fact]
        public void Testwts.ItemNameViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new wts.ItemNameViewModel();
            Assert.NotNull(vm);
        }
        //}]}
    }
}
