using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using RootNamespace.Services;
using RootNamespace.View;
using RootNamespace.ViewModel;

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
