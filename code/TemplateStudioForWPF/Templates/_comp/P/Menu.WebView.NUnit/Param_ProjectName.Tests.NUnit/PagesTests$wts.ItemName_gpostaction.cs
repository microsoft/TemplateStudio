namespace Param_RootNamespace.Tests.NUnit
{
    public class PagesTests
    {
        [SetUp]
        public void Setup()
        {
            // App Services
//{[{
            _container.RegisterType<IRightPaneService, RightPaneService>();
//}]}
        }
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