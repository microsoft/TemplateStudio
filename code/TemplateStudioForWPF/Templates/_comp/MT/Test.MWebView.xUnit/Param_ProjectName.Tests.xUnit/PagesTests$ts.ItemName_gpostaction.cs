namespace Param_RootNamespace.Tests.XUnit
{
    public class PagesTests
    {
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Services
//{[{
            services.AddSingleton<IRightPaneService, RightPaneService>();
//}]}
            // ViewModels
//{[{
            services.AddTransient<ts.ItemNameViewModel>();
//}]}
        }

//{[{
        // TODO: Add tests for functionality you add to ts.ItemNameViewModel.
        [Fact]
        public void Testts.ItemNameViewModelCreation()
        {
            var vm = _host.Services.GetService(typeof(ts.ItemNameViewModel));
            Assert.NotNull(vm);
        }

        [Fact]
        public void TestGetts.ItemNamePageType()
        {
            if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
            {
                var pageType = pageService.GetPageType(typeof(ts.ItemNameViewModel).FullName);
                Assert.Equal(typeof(ts.ItemNamePage), pageType);
            }
            else
            {
                Assert.True(false, $"Can't resolve {nameof(IPageService)}");
            }
        }
//}]}
    }
}
