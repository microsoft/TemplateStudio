using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WtsXPlat.Core.Helpers;
using WtsXPlat.Core.Models;
using WtsXPlat.Core.Services;
using WtsXPlat.Mobile.Services;

namespace WtsXPlat.Mobile.ViewModels
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
