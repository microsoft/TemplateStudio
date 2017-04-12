using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RootNamespace.Services;
using RootNamespace.Views;

namespace RootNamespace.ViewModels
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
