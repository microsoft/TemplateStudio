using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Param_RootNamespace.Services;
using Param_RootNamespace.Views;

namespace Param_RootNamespace.ViewModels
{
    public class ViewModelLocator
    {
        private NavigationServiceEx _navigationService = new NavigationServiceEx();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => _navigationService);
        }

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();
            _navigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
