using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using Param_PageNS.Services;
using Param_PageNS.WebViewPage;

namespace Param_PageNS
{
    public partial class ViewModelLocator
    {
        public WebViewPageViewModel WebViewPageViewModel => ServiceLocator.Current.GetInstance<WebViewPageViewModel>();

        public void RegisterWebViewPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<WebViewPageViewModel>();
            navigationService.Configure(typeof(WebViewPageViewModel).FullName, typeof(WebViewPagePage));
        }
    }
}