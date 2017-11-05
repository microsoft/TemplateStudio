using System;

using GalaSoft.MvvmLight.Ioc;

using LiveTileActivationSample.MVVMLight.Services;
using LiveTileActivationSample.MVVMLight.Views;

using Microsoft.Practices.ServiceLocation;

namespace LiveTileActivationSample.MVVMLight.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            Register<MainViewModel, MainPage>();
            Register<SecondarySectionViewModel, SecondarySectionPage>();
            Register<LiveTileUpdateViewModel, LiveTileUpdatePage>();
        }

        public LiveTileUpdateViewModel LiveTileUpdateViewModel => ServiceLocator.Current.GetInstance<LiveTileUpdateViewModel>();

        public SecondarySectionViewModel SecondarySectionViewModel => ServiceLocator.Current.GetInstance<SecondarySectionViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
