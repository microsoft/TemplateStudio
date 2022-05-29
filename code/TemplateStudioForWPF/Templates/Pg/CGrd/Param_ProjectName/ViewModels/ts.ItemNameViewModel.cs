using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.ViewModels
{
    public class ts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;
        private ICommand _navigateToDetailCommand;

        public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new System.Windows.Input.ICommand<SampleOrder>(NavigateToDetail));

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public ts.ItemNameViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(Param_OnNavigatedToParams)
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await _sampleDataService.GetContentGridDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom(Param_OnNavigatedFromParams)
        {
        }

        private void NavigateToDetail(SampleOrder order)
        {
        }
    }
}
