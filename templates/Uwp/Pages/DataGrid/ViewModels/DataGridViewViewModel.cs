using System;
using System.Collections.ObjectModel;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.Core.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class DataGridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
