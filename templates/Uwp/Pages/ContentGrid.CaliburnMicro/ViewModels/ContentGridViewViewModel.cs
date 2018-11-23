using System;
using System.Collections.ObjectModel;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.Core.Services;
using Param_ItemNamespace.Views;

namespace Param_ItemNamespace.ViewModels
{
    public class ContentGridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetContentGridData();
            }
        }

        public ContentGridViewViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnItemSelected(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
                _navigationService.Navigate(typeof(ContentGridDetailPage), clickedItem);
            }
        }
    }
}
