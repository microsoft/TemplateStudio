using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Services;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace;

// To learn more about MSTests: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest

[TestClass]
public class WebViewParam_RootNamespace
{
    public IWebViewService mockWebViewService;
    public WebViewViewModel webViewViewModel;

    public WebViewParam_RootNamespace()
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