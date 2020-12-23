using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.ViewModels;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ObservableRecipient, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;
        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<SampleOrder>(OnItemClick));

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public wts.ItemNameViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
        {
            _navigationService = navigationService;
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await _sampleDataService.GetContentGridDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom()
        {
        }

        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
                _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
                _navigationService.NavigateTo(typeof(wts.ItemNameDetailViewModel).FullName, clickedItem.OrderID);
            }
        }
    }
}
