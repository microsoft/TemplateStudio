using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace App_Name.App_Page
{
    public class App_PageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public ICommand InitializeCommand { get; private set; }

        public App_PageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            InitializeCommand = new RelayCommand(Initialize);
        }

        private string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle; }
            set
            {
                base.Set(() => PageTitle, ref _pageTitle, value);
            }
        }

        public void Initialize()
        {
            PageTitle = "App_Page Title";
        }
    }
}