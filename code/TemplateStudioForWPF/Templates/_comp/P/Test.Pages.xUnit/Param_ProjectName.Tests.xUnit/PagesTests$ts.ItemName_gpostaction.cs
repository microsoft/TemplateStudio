namespace Param_RootNamespace.Tests.XUnit;

public class PagesTests
{
//^^
//{[{
    // TODO: Add tests for functionality you add to ts.ItemNameViewModel.
    [Fact]
    public void Testts.ItemNameViewModelCreation()
    {
        var vm = _container.Resolve<ts.ItemNameViewModel>();
        Assert.NotNull(vm);
    }
//}]}
}
