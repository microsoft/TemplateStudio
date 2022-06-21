using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;

namespace Param_ProjectName.Tests.MSTest;

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
