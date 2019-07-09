using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.ViewModels
{
    public class DataGridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public DataGridViewViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await SampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }
}
