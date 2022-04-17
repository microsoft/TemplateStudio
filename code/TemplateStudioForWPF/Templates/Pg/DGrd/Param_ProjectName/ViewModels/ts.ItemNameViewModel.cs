using System;
using System.Collections.ObjectModel;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.ViewModels
{
    public class ts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public ts.ItemNameViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(Param_OnNavigatedToParams)
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await _sampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom(Param_OnNavigatedFromParams)
        {
        }
    }
}
