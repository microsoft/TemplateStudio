using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using Windows.UI.Xaml;
using WTSPrism.Models;

namespace WTSPrism.ViewModels
{
    public class MasterDetailDetailPageViewModel : ViewModelBase
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";
        private readonly INavigationService navigationService;

        public ICommand StateChangedCommand { get; }

        private Order item;
        public Order Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        public MasterDetailDetailPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Item = e.Parameter as Order;
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
