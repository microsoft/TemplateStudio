using System;
using System.Collections.ObjectModel;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class ChartViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly ISampleDataService _sampleDataService;

        public ChartViewViewModel(ISampleDataService sampleDataServiceInstance)
        {
            _sampleDataService = sampleDataServiceInstance;
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return _sampleDataService.GetChartSampleData();
            }
        }
    }
}
