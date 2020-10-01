using System.Collections.ObjectModel;
using System.Windows.Input;

using DotNetCoreWpfApp.Contracts.Services;
using DotNetCoreWpfApp.Contracts.ViewModels;
using DotNetCoreWpfApp.Core.Contracts.Services;
using DotNetCoreWpfApp.Core.Models;
using DotNetCoreWpfApp.Helpers;

namespace DotNetCoreWpfApp.ViewModels
{
    public class ContentGridViewModel : Observable, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;
        private ICommand _navigateToDetailCommand;

        public ICommand NavigateToDetailCommand => _navigateToDetailCommand ?? (_navigateToDetailCommand = new RelayCommand<SampleOrder>(NavigateToDetail));

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public ContentGridViewModel(ISampleDataService sampleDataService, INavigationService navigationService)
        {
            _sampleDataService = sampleDataService;
            _navigationService = navigationService;
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

        private void NavigateToDetail(SampleOrder order)
        {
            _navigationService.NavigateTo(typeof(ContentGridDetailViewModel).FullName, order.OrderID);
        }
    }
}
