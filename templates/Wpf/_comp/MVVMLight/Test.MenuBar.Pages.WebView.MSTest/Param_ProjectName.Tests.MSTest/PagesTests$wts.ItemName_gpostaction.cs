namespace Param_RootNamespace.Tests.MSTest
{
    [TestClass]
    public class PagesTests
    {
        public PagesTests()
        {
            // Services
//{[{
            SimpleIoc.Default.Register<IRightPaneService, RightPaneService>();
//}]}
            // Pages
//{[{
            RegisterPage<wts.ItemNameViewModel, wts.ItemNamePage>();
//}]}
        }

        private void RegisterPage<VM, V>()
            where VM : ViewModelBase
            where V : Page
        {
            SimpleIoc.Default.Register<VM>();
            SimpleIoc.Default.Register<V>();
            SimpleIoc.Default.GetInstance<IPageService>().Configure<VM, V>();
        }

//{[{
        // TODO WTS: Add tests for functionality you add to wts.ItemNameViewModel.
        [TestMethod]
        public void Testwts.ItemNameViewModelCreation()
        {
            var vm = SimpleIoc.Default.GetInstance<wts.ItemNameViewModel>();
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void TestGetwts.ItemNamePageType()
        {
            var pageService = SimpleIoc.Default.GetInstance<IPageService>();

            var pageType = pageService.GetPageType(typeof(wts.ItemNameViewModel).FullName);

            Assert.AreEqual(typeof(wts.ItemNamePage), pageType);
        }
//}]}
    }
}