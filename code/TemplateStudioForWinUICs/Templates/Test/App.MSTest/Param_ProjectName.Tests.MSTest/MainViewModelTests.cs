using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class MainParam_RootNamespace
{
    public MainViewModel mainViewModel;

    public MainParam_RootNamespace()
    {
        mainViewModel = new MainViewModel();
    }

    [TestMethod]
    public void TestMethod1()
    {
        //TODO: Add your own tests!
    }   
}
