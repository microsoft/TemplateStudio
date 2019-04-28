using System;
using System.Collections.ObjectModel;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.ViewModels
{
    public class DataGridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private ObservableCollection<SampleOrder> _source;

        public ObservableCollection<SampleOrder> Source
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
            Source = await SampleDataService.GetGridSampleDataAsync();
        }
    }
}
