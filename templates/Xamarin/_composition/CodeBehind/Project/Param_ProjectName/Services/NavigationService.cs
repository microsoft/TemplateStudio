using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Param_RootNamespace.Services
{
    public class NavigationService
    {
        private NavigationPage NavPage => Application.Current.MainPage as NavigationPage;
        private IDictionary<string, Type> viewModelRouting = new Dictionary<string, Type>();
        private static NavigationService _instance;

        public static NavigationService Instance => _instance ?? (_instance = new NavigationService());

        public void Register(string pageKey, Type pageType)
        {
            if (viewModelRouting.ContainsKey(pageKey))
            {
                viewModelRouting[pageKey] = pageType;
            }
            else
            {
                viewModelRouting.Add(pageKey, pageType);
            }
        }

        public async Task NavigateToAsync(string pageKey, object parameters = null)
        {
            Type pageType = viewModelRouting[pageKey];

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