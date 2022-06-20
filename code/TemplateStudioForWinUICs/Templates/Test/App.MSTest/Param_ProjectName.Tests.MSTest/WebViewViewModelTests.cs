using MSTestsRef.Contracts.Services;
using MSTestsRef.Services;
using MSTestsRef.ViewModels;

namespace ViewModelTests;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class WebViewViewModelTests
{
    public IWebViewService mockWebViewService;
    public WebViewViewModel webViewViewModel;

    public WebViewViewModelTests()
    {
        mockWebViewService = new MockWebViewService();
        webViewViewModel = new WebViewViewModel(mockWebViewService);
    }
    [TestMethod]
    public void TestMethod1()
    {
        //TODO: Add your own tests!
    }   
}