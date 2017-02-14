using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace RootNamespace
{
    public partial class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigationService = new Services.NavigationService();
            SimpleIoc.Default.Register(() => navigationService);
            //PostActionAnchor: REGISTER PAGE IN VIEW_MODEL
        }
    }
}
