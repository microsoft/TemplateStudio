using System.Collections.ObjectModel;
using WtsXamarin.Helpers;
using WtsXamarin.Models;
using WtsXamarin.Services;

namespace WtsXamarin.ViewModels
{
    public class ListViewListViewModel : Observable
    {
        private ObservableCollection<SampleOrder> _sampleData;

        public ListViewListViewModel()
        {
            SampleData = SampleDataService.GetGridSampleData();
        }

        public ObservableCollection<SampleOrder> SampleData
        {
            get => _sampleData;
            private set => Set(ref _sampleData, value);
        }

        public SampleOrder SelectedItem
        {
            get => null;
            set => NavigationService.Instance.NavigateToAsync<ListViewItemViewModel>(value);
        }
    }
}
