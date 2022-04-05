namespace Param_RootNamespace.Tests.NUnit
{
    public class PagesTests
    {
//^^
//{[{
        // TODO: Add tests for functionality you add to wts.ItemNameViewModel.
        [Test]
        public void Testwts.ItemNameViewModelCreation()
        {
            var vm = _container.Resolve<wts.ItemNameViewModel>();
            Assert.IsNotNull(vm);
        }
//}]}
    }
}