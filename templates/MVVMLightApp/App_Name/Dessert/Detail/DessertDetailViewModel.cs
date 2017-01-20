using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace App_Name.Dessert.Detail
{
    public class DessertDetailViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        private DessertModel _parameter;
        public DessertModel Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                RaisePropertyChanged(() => Parameter);
            }
        }

        public DessertDetailViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            Messenger.Default.Register<DessertModel>(this, Initialize);
        }

        private void Initialize(DessertModel obj)
        {
            Parameter = obj;

            if (SystemNavigationManager.GetForCurrentView() != null)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

                SystemNavigationManager.GetForCurrentView().BackRequested += ((sender, e) =>
                {
                    _navigationService.GoBack();
                });
            }
        }
    }
}
