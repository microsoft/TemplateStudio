namespace Param_RootNamespace.Tests.xUnit
{
    public class PagesTests
    {
        public PagesTests()
        {
            // App Services
//{[{
            _container.RegisterType<IRightPaneService, RightPaneService>();
//}]}
        }
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