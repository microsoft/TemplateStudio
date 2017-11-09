using System.Collections.ObjectModel;

using Prism.Windows.Mvvm;

using WTSGeneratedNavigation.Models;
using WTSGeneratedNavigation.Services;

namespace WTSGeneratedNavigation.ViewModels
{
    public class GridViewModel : ViewModelBase
    {
        private readonly ISampleDataService sampleDataService;

        public GridViewModel(ISampleDataService sampleDataServiceInstance)
        {
            sampleDataService = sampleDataServiceInstance;
        }

        public ObservableCollection<SampleOrder> Source => sampleDataService.GetGridSampleData();
    }
}
