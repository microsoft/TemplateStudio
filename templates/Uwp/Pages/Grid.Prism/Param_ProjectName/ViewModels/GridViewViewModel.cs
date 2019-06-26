using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Windows.Mvvm;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace Param_RootNamespace.ViewModels
{
    public class GridViewViewModel : ViewModelBase
    {
        private readonly ISampleDataService _sampleDataService;
        private ObservableCollection<SampleOrder> _source;

        public GridViewViewModel(ISampleDataService sampleDataServiceInstance)
        {
            _sampleDataService = sampleDataServiceInstance;
        }

        public ObservableCollection<SampleOrder> Source
        {
            get
            {
                return _source;
            }

            set
            {
                SetProperty(ref _source, value);
            }
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            // TODO WTS: Replace this with your actual data
            Source = await _sampleDataService.GetGridDataAsync();
        }
    }
}
