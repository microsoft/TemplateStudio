using System;
using System.Collections.Generic;
using WtsXamarin.ViewModels;
using WtsXamarin.Views;
using WtsXamarin.Views.Navigation;
using Xamarin.Forms;

namespace WtsXamarin.Services
{

    public class NavigationService
    {
        private static NavigationService _instance;
        private IDictionary<Type, Type> viewModelRouting = new Dictionary<Type, Type>()
        {
            { typeof(MainViewModel), typeof(MainPage) },
            { typeof(BlankViewModel), typeof(BlankPage) },
            { typeof(WebViewViewModel), typeof(WebViewPage) },
            { typeof(ListViewViewModel), typeof(ListViewPage) },
            { typeof(CameraViewModel), typeof(CameraPage) },
            { typeof(ListViewMasterViewModel), typeof(ListViewMasterPage) },
            { typeof(ListViewDetailViewModel), typeof(ListViewDetailPage) },
            { typeof(SettingsViewModel), typeof(SettingsPage) },
        };

        public static NavigationService Instance => _instance ?? (_instance = new NavigationService());


        public void NavigateTo<TDestinationViewModel>(object parameters = null)
        {
            Type pageType = viewModelRouting[typeof(TDestinationViewModel)];
            NavigateTo(pageType, parameters);
        }

        public void NavigateTo(MasterDetailPageMenuItem menuItem, object parameters = null)
        {
            if (menuItem == null || menuItem.TargetType == null)
                return;

            NavigateTo(menuItem.TargetType, parameters);
        }

        private void NavigateTo(Type pageType, object parameters = null)
        {
            if ((parameters != null
                ? Activator.CreateInstance(pageType, parameters)
                : Activator.CreateInstance(pageType)) is Page page)
                App.NavPage.PushAsync(page);
        }
    }
}
