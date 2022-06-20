using MSTestsRef.ViewModels;

namespace ViewModelTests;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class MainViewModelTests
{
    public MainViewModel mainViewModel;

    public MainViewModelTests()
    {
        mainViewModel = new MainViewModel();
    }

    [TestMethod]
    public void TestMethod1()
    {
        //TODO: Add your own tests!
    }   
}