using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using WTSPrismNavigationBase.Models;
using WTSPrismNavigationBase.Services;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Diagnostics;
using Prism;
using WTSPrismNavigationBase.Behaviors;

namespace WTSPrismNavigationBase.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState _currentState;

        private Order _selected;
        public Order Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        private INavigationService navigationService;

        public ICommand ItemClickCommand { get; private set; }
        public ICommand StateChangedCommand { get; private set; }
        public ICommand SetInitialStateCommand { get; private set; }

        public ObservableCollection<Order> SampleItems { get; private set; } = new ObservableCollection<Order>();

        public MasterDetailPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            ItemClickCommand = new DelegateCommand<ItemClickEventArgs>(OnItemClick);
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(OnStateChanged);
            SetInitialStateCommand = new DelegateCommand<VisualState>(SetInitialState);
        }

        public async override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            SampleItems.Clear();

            var data = await SampleDataService.GetSampleModelDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }
            Selected = SampleItems.First();
        }

        private void SetInitialState(VisualState state)
        {
            _currentState = state;
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            Order item = args?.ClickedItem as Order;
            if (item != null)
            {
                if (_currentState?.Name == NarrowStateName)
                {
                    navigationService.Navigate("MasterDetailDetail", item);
                }
                else
                {
                    Selected = item;
                }
            }
        }

        public void OnPivotNavigatedFrom()
        {
            
        }

        public async void OnPivotNavigatedTo()
        {
            await LoadDataAsync();
        }
    }
}
