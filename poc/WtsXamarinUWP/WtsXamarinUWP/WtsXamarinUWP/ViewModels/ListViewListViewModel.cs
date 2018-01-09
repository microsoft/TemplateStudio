using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WtsXamarinUWP.Core.Helpers;
using WtsXamarinUWP.Core.Models;
using WtsXamarinUWP.Core.Services;
using WtsXamarinUWP.Mobile.Services;

namespace WtsXamarinUWP.Mobile.ViewModels
{
    public class ListViewListViewModel : Observable
    {
        private ObservableCollection<SampleOrder> _sampleData;

        public ListViewListViewModel()
        {
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

        public async Task LoadDataAsync()
        {
            var data = await SampleDataService.GetAllOrdersAsync();
            SampleData = new ObservableCollection<SampleOrder>(data);
        }
    }
}
