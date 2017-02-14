using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using RootNamespace.Services;
using RootNamespace.ItemName;

namespace RootNamespace
{
    public partial class ViewModelLocator
    {
        public ItemNameViewModel ItemNameViewModel => ServiceLocator.Current.GetInstance<ItemNameViewModel>();

        public void RegisterItemName(NavigationService navigationService)
        {
            SimpleIoc.Default.Register<ItemNameViewModel>();
            navigationService.Configure(typeof(ItemNameViewModel).FullName, typeof(ItemNamePage));
        }
    }
}