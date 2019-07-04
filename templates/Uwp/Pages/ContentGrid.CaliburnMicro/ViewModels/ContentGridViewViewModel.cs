using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Services;
using Param_RootNamespace.Views;

namespace Param_RootNamespace.ViewModels
{
    public class ContentGridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IConnectedAnimationService _connectedAnimationService;

        private ObservableCollection<SampleOrder> _source;

        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                return _source;
            }

            set
            {
                Set(ref _source, value);
            }
        }

        public ContentGridViewViewModel(INavigationService navigationService, IConnectedAnimationService connectedAnimationService)
        {
            _navigationService = navigationService;
            _connectedAnimationService = connectedAnimationService;
        }

        public async Task LoadDataAsync()
        {
            // TODO WTS: Replace this with your actual data
            Source = await SampleDataService.GetContentGridDataAsync();
        }

        public void OnItemSelected(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
                _connectedAnimationService.SetListDataItemForNextConnectedAnimation(clickedItem);
                _navigationService.Navigate(typeof(ContentGridViewDetailPage), clickedItem.OrderID);
            }
        }
    }
}
