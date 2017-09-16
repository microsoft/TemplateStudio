using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using WTSPrism.Models;
using WTSPrism.Services;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Diagnostics;
using Prism;
using WTSPrism.Behaviors;

namespace WTSPrism.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase, IPivotNavigationAware
    {
        private readonly INavigationService navigationService;
        private readonly ISampleDataService sampleDataService;

        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState currentState;

        public MasterDetailPageViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
        {
            this.navigationService = navigationService;
            this.sampleDataService = sampleDataService;
            ItemClickCommand = new DelegateCommand<ItemClickEventArgs>(OnItemClick);
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(OnStateChanged);
            SetInitialStateCommand = new DelegateCommand<VisualState>(SetInitialState);
        }

        private Order selected;
        public Order Selected
        {
            get => selected;
            set => SetProperty(ref selected, value);
        }

        public ICommand ItemClickCommand { get; private set; }
        public ICommand StateChangedCommand { get; private set; }
        public ICommand SetInitialStateCommand { get; private set; }

        public ObservableCollection<Order> SampleItems { get; private set; } = new ObservableCollection<Order>();

        public async Task LoadDataAsync(VisualState currentState)
        {
            this.currentState = currentState;
            SampleItems.Clear();

            var data = await sampleDataService.GetSampleModelDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }
            Selected = SampleItems.First();
        }

        private void SetInitialState(VisualState state)
        {
            currentState = state;
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            currentState = args.NewState;
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            Order item = args?.ClickedItem as Order;
            if (item != null)
            {
                if (currentState?.Name == NarrowStateName)
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
            await LoadDataAsync(currentState);
        }
    }
}
