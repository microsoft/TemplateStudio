using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RootNamespace.Services;
using RootNamespace.Views;
using RootNamespace.ViewModels;

namespace RootNamespace
{
    public class ViewModelLocator
    {
        NavigationServiceEx _navigationService = new NavigationServiceEx();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => _navigationService);
        }

        public void Register<VM, V>() where VM : class
        {
            SimpleIoc.Default.Register<VM>();
            _navigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
