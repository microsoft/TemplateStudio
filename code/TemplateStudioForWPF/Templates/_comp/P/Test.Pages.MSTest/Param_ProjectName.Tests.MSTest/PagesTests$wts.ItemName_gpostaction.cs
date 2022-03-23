namespace Param_RootNamespace.Tests.MSTest
{
    [TestClass]
    public class PagesTests
    {
//^^
//{[{
        // TODO WTS: Add tests for functionality you add to wts.ItemNameViewModel.
        [TestMethod]
        public void Testwts.ItemNameViewModelCreation()
        {
            var vm = _container.Resolve<wts.ItemNameViewModel>();
            Assert.IsNotNull(vm);
        }
//}]}
    }
}