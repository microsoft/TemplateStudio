using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RootNamespace.Services;
using RootNamespace.Views;
using RootNamespace.ViewModels;

namespace RootNamespace
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = new NavigationServiceEx();
            SimpleIoc.Default.Register(() => navigationService);
        }
    }
}
