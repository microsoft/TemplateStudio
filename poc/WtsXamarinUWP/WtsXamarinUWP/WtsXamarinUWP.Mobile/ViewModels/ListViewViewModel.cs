using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WtsXamarinUWP.Core.Helpers;
using WtsXamarinUWP.Core.Models;
using WtsXamarinUWP.Core.Services;

namespace WtsXamarinUWP.Mobile.ViewModels
{
    public class ListViewViewModel : Observable
    {
        private ObservableCollection<SampleOrder> _sampleData;
        private SampleOrder _selectedItem;

        public ListViewViewModel()
        {            
        }

        public ObservableCollection<SampleOrder> SampleData
        {
            get => _sampleData;
            private set => Set(ref _sampleData, value);
        }

        public SampleOrder SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        public async Task LoadDataAsync()
        {
            var data = await SampleDataService.GetAllOrdersAsync();
            SampleData = new ObservableCollection<SampleOrder>(data);
        }
    }
}
