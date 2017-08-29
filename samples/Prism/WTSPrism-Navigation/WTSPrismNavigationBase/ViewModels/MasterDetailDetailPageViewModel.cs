using Prism.Commands;
using Prism.Windows.Mvvm;
using System;
using System.Windows.Input;

using Windows.UI.Xaml;

using WTSPrismNavigationBase.Models;
using WTSPrismNavigationBase.Services;
using Prism.Windows.Navigation;
using System.Collections.Generic;

namespace WTSPrismNavigationBase.ViewModels
{
    public class MasterDetailDetailPageViewModel : ViewModelBase
    {

        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";
        private INavigationService navigationService;

        public ICommand StateChangedCommand { get; private set; }

        private Order _item;
        public Order Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
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
