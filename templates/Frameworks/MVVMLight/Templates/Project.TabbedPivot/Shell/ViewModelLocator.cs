using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using ItemName.Shell;
using ItemName.Services;

namespace ItemName
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