using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using uct.ItemName.Shell;
using uct.ItemName.Services;

namespace uct.ItemName
{
    public partial class ViewModelLocator
    {
        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public void RegisterShell(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<ShellViewModel>();
            navigationService.Configure(typeof(ShellViewModel).FullName, typeof(ShellPage));
        }
    }
}