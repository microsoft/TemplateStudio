using System.Collections.ObjectModel;

using Prism.Windows.Mvvm;

using WTSGeneratedPivot.Models;
using WTSGeneratedPivot.Services;

namespace WTSGeneratedPivot.ViewModels
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
