using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using RootNamespace.Services;
using RootNamespace.uct.ItemName;

namespace RootNamespace
{
    public partial class ViewModelLocator
    {
        public uct.ItemNameViewModel uct.ItemNameViewModel => ServiceLocator.Current.GetInstance<uct.ItemNameViewModel>();

        public void Registeruct.ItemName(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<uct.ItemNameViewModel>();
            navigationService.Configure(typeof(uct.ItemNameViewModel).FullName, typeof(uct.ItemNamePage));
        }
    }
}