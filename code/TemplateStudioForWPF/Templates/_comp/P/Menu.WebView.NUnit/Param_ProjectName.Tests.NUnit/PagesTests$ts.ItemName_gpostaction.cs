namespace Param_RootNamespace.Tests.NUnit;

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
    // TODO: Add tests for functionality you add to ts.ItemNameViewModel.
    [Test]
    public void Testts.ItemNameViewModelCreation()
    {
        var vm = _container.Resolve<ts.ItemNameViewModel>();
        Assert.IsNotNull(vm);
    }
//}]}
}
