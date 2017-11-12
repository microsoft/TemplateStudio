using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using WTSPrismNavigationBase.Models;
using WTSPrismNavigationBase.Services;

namespace WTSPrismNavigationBase.ViewModels
{
    public class MasterDetailDetailPageViewModel : ViewModelBase
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private readonly INavigationService navigationService;
        private readonly ISampleDataService sampleDataService;

        public ICommand StateChangedCommand { get; }

        private Order item;
        public Order Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public MasterDetailDetailPageViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
        {
            this.navigationService = navigationService;
            this.sampleDataService = sampleDataService;
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
