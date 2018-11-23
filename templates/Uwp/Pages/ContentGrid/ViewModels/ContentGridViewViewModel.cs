using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.Core.Services;
using Param_ItemNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class ContentGridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private ICommand _itemClickCommand;        

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<SampleOrder>(OnItemClick));

        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetContentGridData();
            }
        }

        public ContentGridViewViewModel()
        {
        }

        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
            }
        }
    }
}
