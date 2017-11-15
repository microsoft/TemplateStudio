using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Windows.UI.Xaml;

using WTSGeneratedPivot.Models;
using WTSGeneratedPivot.Services;

namespace WTSGeneratedPivot.ViewModels
{
    public class MasterDetailDetailViewModel : ViewModelBase
    {
        private const string NarrowStateName = "NarrowState";
        private const string WideStateName = "WideState";

        private readonly INavigationService navigationService;
        private readonly ISampleDataService sampleDataService;

        public ICommand StateChangedCommand { get; }

        private SampleOrder item;

        public SampleOrder Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public MasterDetailDetailViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance)
        {
            navigationService = navigationServiceInstance;
            sampleDataService = sampleDataServiceInstance;
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            long orderId = (long)e.Parameter;
            var data = await sampleDataService.GetSampleModelDataAsync();
            Item = data.FirstOrDefault(x => x.OrderId == orderId);
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                navigationService.GoBack();
            }
        }
    }
}
