using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using App_Name.Dessert.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace App_Name.Dessert.List
{
    public class DessertListViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        public ICommand LoadDataCommand { get; private set; }
        public ICommand NavigateToDetailCommand { get; private set; }

        public ObservableCollection<DessertModel> DessertList { get; private set; } = new ObservableCollection<DessertModel>();

        public DessertListViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            LoadDataCommand = new RelayCommand(async ()=> { await LoadDataAsync(); });
            NavigateToDetailCommand = new RelayCommand<ItemClickEventArgs>(NavigateToDetail);
        }

        public async Task LoadDataAsync()
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
                Messenger.Default.Send<DessertModel>(selected);
                _navigationService.NavigateTo(typeof(DessertDetailViewModel).FullName, selected);
            }
        }
    }
}
