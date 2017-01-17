using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Ioc;
using SplitViewTemplate.View;

namespace SplitViewTemplate.ViewModel
{
    public class ViewModelLocator
    {
        public const string HomeViewKey = "HomeView";
        public const string PowerPointViewKey = "PowerPointView";
        public const string ExcelViewKey = "ExcelView";
        public const string WordViewKey = "WordView";
        public const string AboutViewKey = "AboutView";

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //Services registration
            var navigationService = new Services.NavigationService();
            navigationService.Configure(HomeViewKey, typeof(HomeView));
            navigationService.Configure(PowerPointViewKey, typeof(PowerPointView));
            navigationService.Configure(ExcelViewKey, typeof(ExcelView));
            navigationService.Configure(WordViewKey, typeof(WordView));            
            navigationService.Configure(AboutViewKey, typeof(AboutView));
            SimpleIoc.Default.Register<Services.INavigationService>(() => navigationService);

            //ViewModel registration
            SimpleIoc.Default.Register<ShellViewModel>();
        }        
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();
    }
}
