using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace MVVMLightBlankProject
{
    public partial class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = new Services.NavigationService();
            SimpleIoc.Default.Register<Services.NavigationService>(() => navigationService);
                        
            RegisterHome(navigationService);
            //TODO: Register new app sections
        }
    }
}
