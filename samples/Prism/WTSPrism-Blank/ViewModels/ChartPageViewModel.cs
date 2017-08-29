using Prism.Windows.Mvvm;
using System;
using System.Collections.ObjectModel;

using WTSPrism.Models;
using WTSPrism.Services;

namespace WTSPrism.ViewModels
{
    public class ChartPageViewModel : ViewModelBase
    {
        public ChartPageViewModel()
        {
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetChartSampleData();
            }
        }
    }
}
