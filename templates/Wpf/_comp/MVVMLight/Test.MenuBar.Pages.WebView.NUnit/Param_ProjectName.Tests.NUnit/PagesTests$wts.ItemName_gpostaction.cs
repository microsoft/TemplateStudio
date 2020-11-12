namespace Param_RootNamespace.Tests.NUnit
{
    public class PagesTests
    {
        [SetUp]
        public void Setup()
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
        [Test]
        public void Testwts.ItemNameViewModelCreation()
        {
            var vm = SimpleIoc.Default.GetInstance<wts.ItemNameViewModel>();
            Assert.IsNotNull(vm);
        }

        [Test]
        public void TestGetwts.ItemNamePageType()
        {
            var pageService = SimpleIoc.Default.GetInstance<IPageService>();

            var pageType = pageService.GetPageType(typeof(wts.ItemNameViewModel).FullName);

            Assert.AreEqual(typeof(wts.ItemNamePage), pageType);
        }
//}]}
    }
}