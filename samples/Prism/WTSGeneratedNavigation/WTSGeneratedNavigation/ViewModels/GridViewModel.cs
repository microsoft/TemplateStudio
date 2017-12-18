using System.Collections.ObjectModel;

using Prism.Windows.Mvvm;

using WTSGeneratedNavigation.Models;
using WTSGeneratedNavigation.Services;

namespace WTSGeneratedNavigation.ViewModels
{
    public class GridViewModel : ViewModelBase
    {
        private readonly ISampleDataService _sampleDataService;

        public GridViewModel(ISampleDataService sampleDataServiceInstance)
        {
            _sampleDataService = sampleDataServiceInstance;
        }

        public ObservableCollection<SampleOrder> Source => _sampleDataService.GetGridSampleData();
    }
}
