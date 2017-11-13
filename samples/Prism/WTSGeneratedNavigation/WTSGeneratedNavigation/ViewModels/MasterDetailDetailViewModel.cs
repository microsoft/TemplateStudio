using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Windows.UI.Xaml;

using WTSGeneratedNavigation.Models;
using WTSGeneratedNavigation.Services;

namespace WTSGeneratedNavigation.ViewModels
{
    public class MasterDetailDetailViewModel : ViewModelBase
    {
        private const string NarrowStateName = "NarrowState";
        private const string WideStateName = "WideState";

        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;

        public ICommand StateChangedCommand { get; }

        private SampleOrder _item;

        public SampleOrder Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        public MasterDetailDetailViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance)
        {
            _navigationService = navigationServiceInstance;
            _sampleDataService = sampleDataServiceInstance;
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            long orderId = (long)e.Parameter;
            var data = await _sampleDataService.GetSampleModelDataAsync();
            Item = data.FirstOrDefault(x => x.OrderId == orderId);
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                _navigationService.GoBack();
            }
        }
    }
}
