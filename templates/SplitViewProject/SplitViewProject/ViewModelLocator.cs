using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace SplitViewProject
{
    public partial class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = new Services.NavigationService();
            SimpleIoc.Default.Register<Services.NavigationService>(() => navigationService);

            RegisterShell();
            RegisterHome(navigationService);
            //PostActionAnchor: REGISTER PAGE IN VIEW_MODEL
        }
    }
}
