using System;
using System.Collections.ObjectModel;

using Prism.Windows.Mvvm;

using WTSGeneratedPivot.Models;
using WTSGeneratedPivot.Services;

namespace WTSGeneratedPivot.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        private readonly ISampleDataService sampleDataService;

        public ChartViewModel(ISampleDataService sampleDataServiceInstance)
        {
            sampleDataService = sampleDataServiceInstance;
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return sampleDataService.GetChartSampleData();
            }
        }
    }
}
