using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class GridViewViewModel : ViewModelBase
    {
        private readonly ISampleDataService _sampleDataService;

        public GridViewViewModel(ISampleDataService sampleDataServiceInstance)
        {
            _sampleDataService = sampleDataServiceInstance;
        }

        public ObservableCollection<SampleOrder> Source => _sampleDataService.GetGridSampleData();
    }
}
