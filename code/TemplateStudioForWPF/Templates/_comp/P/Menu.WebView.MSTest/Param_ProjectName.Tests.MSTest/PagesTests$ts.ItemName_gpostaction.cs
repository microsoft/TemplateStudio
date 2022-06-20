namespace Param_RootNamespace.Tests.MSTest;

[TestClass]
public class PagesTests
{
    public PagesTests()
    {
        // App Services
//{[{
        _container.RegisterType<IRightPaneService, RightPaneService>();
//}]}
    }
//^^
//{[{
    // TODO: Add tests for functionality you add to ts.ItemNameViewModel.
    [TestMethod]
    public void Testts.ItemNameViewModelCreation()
    {
        var vm = _container.Resolve<ts.ItemNameViewModel>();
        Assert.IsNotNull(vm);
    }
//}]}
}
