namespace Param_RootNamespace.Tests.MSTest;

[TestClass]
public class PagesTests
{
    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // ViewModels
//{[{
        services.AddTransient<ts.ItemNameViewModel>();
//}]}
    }

//{[{
    // TODO: Add tests for functionality you add to ts.ItemNameViewModel.
    [TestMethod]
    public void Testts.ItemNameViewModelCreation()
    {
        var vm = _host.Services.GetService(typeof(ts.ItemNameViewModel));
        Assert.IsNotNull(vm);
    }

    [TestMethod]
    public void TestGetts.ItemNamePageType()
    {
        if (_host.Services.GetService(typeof(IPageService)) is IPageService pageService)
        {
            var pageType = pageService.GetPageType(typeof(ts.ItemNameViewModel).FullName);
            Assert.AreEqual(typeof(ts.ItemNamePage), pageType);
        }
        else
        {
            Assert.Fail($"Can't resolve {nameof(IPageService)}");
        }
    }
//}]}
}
