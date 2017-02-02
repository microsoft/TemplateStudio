using GalaSoft.MvvmLight.Ioc;
using Param_PageNS.Services;
using Param_PageNS.BlankPage;

namespace Param_PageNS
{
    public partial class ViewModelLocator
    {
        public BlankPageViewModel BlankPageViewModel => ServiceLocator.Current.GetInstance<BlankPageViewModel>();

        public void RegisterBlankPage(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<BlankPageViewModel>();
            navigationService.Configure(typeof(BlankPageViewModel).FullName, typeof(BlankPagePage));
        }
    }
}