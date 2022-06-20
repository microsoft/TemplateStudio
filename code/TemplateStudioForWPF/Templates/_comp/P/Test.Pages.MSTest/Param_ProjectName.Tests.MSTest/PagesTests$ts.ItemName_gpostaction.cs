namespace Param_RootNamespace.Tests.MSTest;

[TestClass]
public class PagesTests
{
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
