using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using WTSGeneratedPivot.Models;
using WTSGeneratedPivot.Services;

namespace WTSGeneratedPivot.ViewModels
{
    public class MasterDetailViewModel : ViewModelBase
    {
        private const string NarrowStateName = "NarrowState";
        private const string WideStateName = "WideState";

        private readonly INavigationService navigationService;
        private readonly ISampleDataService sampleDataService;

        private VisualState currentState;

        public MasterDetailViewModel(INavigationService navigationServiceInstance, ISampleDataService sampleDataServiceInstance)
        {
            navigationService = navigationServiceInstance;
            sampleDataService = sampleDataServiceInstance;
            ItemClickCommand = new DelegateCommand<ItemClickEventArgs>(OnItemClick);
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(OnStateChanged);
            LoadedCommand = new DelegateCommand<VisualState>(OnLoaded);
        }

        private SampleOrder selected;

        public SampleOrder Selected
        {
            get => selected;
            set => SetProperty(ref selected, value);
        }

        public ICommand ItemClickCommand { get; }

        public ICommand StateChangedCommand { get; }

        public ICommand LoadedCommand { get; }

        public ObservableCollection<SampleOrder> SampleItems { get; } = new ObservableCollection<SampleOrder>();

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            await LoadDataAsync(currentState);
        }

        public async Task LoadDataAsync(VisualState currentStateValue)
        {
            currentState = currentStateValue;
            SampleItems.Clear();

            var data = await sampleDataService.GetSampleModelDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            Selected = SampleItems.First();
        }

        private void OnLoaded(VisualState state)
        {
            currentState = state;
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            currentState = args.NewState;
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            if (args?.ClickedItem is SampleOrder item)
            {
                if (currentState?.Name == NarrowStateName)
                {
                    navigationService.Navigate(PageTokens.MasterDetailDetailPage, item.OrderId);
                }
                else
                {
                    Selected = item;
                }
            }
        }
    }
}
