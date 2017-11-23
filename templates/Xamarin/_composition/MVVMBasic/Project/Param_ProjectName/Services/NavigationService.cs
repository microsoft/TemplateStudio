using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Param_RootNamespace.Services
{
    public class NavigationService
    {
        private NavigationPage NavPage => Application.Current.MainPage as NavigationPage;
        private IDictionary<Type, Type> viewModelRouting = new Dictionary<Type, Type>();
        private static NavigationService _instance;

        public static NavigationService Instance => _instance ?? (_instance = new NavigationService());

        public void Register<TDestinationViewModel>(Type pageType)
        {
            var pageKey = typeof(TDestinationViewModel);

            if (viewModelRouting.ContainsKey(pageKey))
            {
                viewModelRouting[pageKey] = pageType;
            }
            else
            {
                viewModelRouting.Add(pageKey, pageType);
            }
        }

        public async Task NavigateToAsync<TDestinationViewModel>(object parameters = null)
        {
            Type pageType = viewModelRouting[typeof(TDestinationViewModel)];

            if ((parameters != null
                ? Activator.CreateInstance(pageType, parameters)
                : Activator.CreateInstance(pageType)) is Page page)
                await NavPage.PushAsync(page);
        }

        public async Task GoBack()
        {
            await NavPage.PopAsync();
        }        
    }
}