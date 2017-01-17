using Caliburn.Micro;
using CaliburnMicroApp.Dessert.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CaliburnMicroApp.Dessert.List
{
    public class DessertListViewModel : Screen
    {
        private INavigationService _navigationService;
        public ObservableCollection<DessertModel> DessertList { get; private set; } = new ObservableCollection<DessertModel>();

        public DessertListViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async void LoadDataAsync()
        {
            DessertList.Clear();

            var service = new DessertService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                DessertList.Add(item);
            }
        }

        public void NavigateToDetail(ItemClickEventArgs param)
        {
            var selected = param.ClickedItem as DessertModel;
            if (selected != null)
            {
                _navigationService.NavigateToViewModel<DessertDetailViewModel>(selected);
            }
        }
    }
}
