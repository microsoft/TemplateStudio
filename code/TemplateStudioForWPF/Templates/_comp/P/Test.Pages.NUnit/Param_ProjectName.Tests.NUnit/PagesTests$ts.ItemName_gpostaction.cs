namespace Param_RootNamespace.Tests.NUnit;

public class PagesTests
{
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
