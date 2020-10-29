namespace Param_RootNamespace.Tests.XUnit
{
    public class PagesTests
    {
//^^
//{[{
        // TODO WTS: Add tests for functionality you add to wts.ItemNameViewModel.
        [Fact]
        public void Testwts.ItemNameViewModelCreation()
        {
            var vm = _container.Resolve<wts.ItemNameViewModel>();
            Assert.NotNull(vm);
        }
//}]}
    }
}