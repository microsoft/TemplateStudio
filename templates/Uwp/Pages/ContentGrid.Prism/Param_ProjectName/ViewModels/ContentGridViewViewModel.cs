using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Navigation;
using Param_ItemNamespace.Core.Models;
using Param_ItemNamespace.Core.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class ContentGridViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;

        private ObservableCollection<SampleOrder> _source;
        private ICommand _itemClickCommand;

        public ObservableCollection<SampleOrder> Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new DelegateCommand<SampleOrder>(OnItemClick));

        public ContentGridViewViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance)
        {
            _navigationService = navigationServiceInstance;
            _sampleDataService = sampleDataServiceInstance;

            // TODO WTS: Replace this with your actual data
            Source = _sampleDataService.GetContentGridData();
        }

        private void OnItemClick(SampleOrder clickedItem)
        {
            if (clickedItem != null)
            {
                _navigationService.Navigate(PageTokens.ContentGridViewDetailPage, clickedItem);
            }
        }
    }
}
