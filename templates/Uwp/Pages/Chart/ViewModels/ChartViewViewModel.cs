using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ChartViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private ObservableCollection<DataPoint> _source;

        public ChartViewViewModel()
        {
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                return _source;
            }

            set
            {
                Param_Setter(ref _source, value);
            }
        }

        public async Task LoadDataAsync()
        {
            // TODO WTS: Replace this with your actual data
            Source = await SampleDataService.GetChartDataAsync();
        }
    }
}
