using System.Collections.ObjectModel;
using XamarinUwpNative.Helpers;
using XamarinUwpNative.Models;
using XamarinUwpNative.Services;

namespace XamarinUwpNative.ViewModels
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
