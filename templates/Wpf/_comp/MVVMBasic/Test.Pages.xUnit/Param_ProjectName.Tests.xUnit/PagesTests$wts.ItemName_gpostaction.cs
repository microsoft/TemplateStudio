namespace Param_RootNamespace.Tests.xUnit
{
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
        // TODO WTS: Add tests for functionality you add to wts.ItemNameViewModel.
        [Fact]
        public void Testwts.ItemNameViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(wts.ItemNameViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetwts.ItemNamePageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(wts.ItemNameViewModel).FullName);
                Assert.Equal(typeof(wts.ItemNamePage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }
//}]}
    }
}