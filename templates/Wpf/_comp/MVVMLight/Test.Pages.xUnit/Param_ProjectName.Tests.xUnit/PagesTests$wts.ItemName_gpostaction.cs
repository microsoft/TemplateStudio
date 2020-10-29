namespace Param_RootNamespace.Tests.XUnit
{
    public class PagesTests
    {
        public PagesTests()
        {
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
        [Fact]
        public void Testwts.ItemNameViewModelCreation()
        {
            var vm = SimpleIoc.Default.GetInstance<wts.ItemNameViewModel>();
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetwts.ItemNamePageType()
        {
            var pageService = SimpleIoc.Default.GetInstance<IPageService>();

            var pageType = pageService.GetPageType(typeof(wts.ItemNameViewModel).FullName);

            Assert.Equal(typeof(wts.ItemNamePage), pageType);
        }
//}]}
    }
}