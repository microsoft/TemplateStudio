using Caliburn.Micro;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace App_Name.App_Page
{
    public class App_PageViewModel : Screen
    {
        private INavigationService _navigationService;

        public App_PageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                _pageTitle = value;
                NotifyOfPropertyChange(() => PageTitle);
            }
        }

        public void Initialize()
        {
            PageTitle = "App_Page Title";
        }
    }
}
