using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using WTSPrism.Models;
using WTSPrism.Services;

namespace WTSPrism.ViewModels
{
    public class GridPageViewModel : ViewModelBase
    {
        private readonly ISampleDataService sampleDataService;

        public GridPageViewModel(ISampleDataService sampleDataService)
        {
            this.sampleDataService = sampleDataService;
        }

        public ObservableCollection<Order> Source => sampleDataService.GetGridSampleData();
    }
}
