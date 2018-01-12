using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Wts.Core.Helpers;
using Wts.Core.Models;
using Wts.Core.Services;

namespace Wts.Mobile.ViewModels
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
