using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

using VideoResume.MVVMLight.Services;
using VideoResume.MVVMLight.Views;

namespace VideoResume.MVVMLight.ViewModels
{
    public class ViewModelLocator
    {
        NavigationServiceEx _navigationService = new NavigationServiceEx();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => _navigationService);
            Register<MediaPlayerViewModel, MediaPlayerPage>();
        }

        public MediaPlayerViewModel MediaPlayerViewModel => ServiceLocator.Current.GetInstance<MediaPlayerViewModel>();

        public void Register<VM, V>() where VM : class
        {
            SimpleIoc.Default.Register<VM>();
            _navigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
