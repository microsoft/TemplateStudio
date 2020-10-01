using System.Collections.ObjectModel;

using DotNetCoreWpfApp.Contracts.ViewModels;
using DotNetCoreWpfApp.Core.Contracts.Services;
using DotNetCoreWpfApp.Core.Models;
using DotNetCoreWpfApp.Helpers;

namespace DotNetCoreWpfApp.ViewModels
{
    public class DataGridViewModel : Observable, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public DataGridViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await _sampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
