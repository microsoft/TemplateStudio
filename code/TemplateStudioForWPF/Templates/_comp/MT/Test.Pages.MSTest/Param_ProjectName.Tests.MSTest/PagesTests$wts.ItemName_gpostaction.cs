namespace Param_RootNamespace.Tests.MSTest
{
    [TestClass]
    public class PagesTests
    {
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // ViewModels
//{[{
            services.AddTransient<wts.ItemNameViewModel>();
//}]}
        }

//{[{
        // TODO: Add tests for functionality you add to wts.ItemNameViewModel.
        [TestMethod]
        public void Testwts.ItemNameViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(wts.ItemNameViewModel));
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void TestGetwts.ItemNamePageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(wts.ItemNameViewModel).FullName);
                Assert.AreEqual(typeof(wts.ItemNamePage), pageType);
            }
            else
            {
                Assert.Fail($"Can't resolve {nameof(IPageService)}");
            }
        }
//}]}
    }
}