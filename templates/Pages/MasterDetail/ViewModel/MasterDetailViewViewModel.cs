using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ItemNamespace.Model;
using ItemNamespace.Services;

namespace ItemNamespace.ViewModel
{
    public class MasterDetailViewViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        const string NarrowStateName = "NarrowState";
        const string DefaultStateName = "DefaultState";

        private VisualState _currentState;

        private Visibility _masterVisibility;
        public Visibility MasterVisibility
        {
            get { return _masterVisibility; }
            set { Set(ref _masterVisibility, value); }
        }

        private Visibility _detailVisibility = Visibility.Collapsed;
        public Visibility DetailVisibility
        {
            get { return _detailVisibility; }
            set { Set(ref _detailVisibility, value); }
        }

        private SampleModel _selected;
        public SampleModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
        
        public ICommand ItemClickCommand { get; private set; }

        public ObservableCollection<SampleModel> SampleItems { get; private set; } = new ObservableCollection<SampleModel>();

        public MasterDetailViewViewModel()
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            SetGoBack();
        }        

        public async Task LoadDataAsync(VisualState currentState)
        {
            _currentState = currentState;
            SampleItems.Clear();

            var service = new SampleModelService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }
            Selected = SampleItems.First();
            NavigateToDetail();
        }

        public void UpdateWindowState(VisualStateChangedEventArgs args)
        {
            _currentState = args.NewState;
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == DefaultStateName)
            {
                MasterVisibility = Visibility.Visible;
                DetailVisibility = Visibility.Visible;
            }
            else
            {
                MasterVisibility = Visibility.Visible;
                DetailVisibility = Visibility.Collapsed;
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            SampleModel item = args?.ClickedItem as SampleModel;
            if (item != null)
            {
                Selected = item;
                NavigateToDetail();
            }
        }

        private void NavigateToDetail()
        {
            DetailVisibility = Visibility.Visible;
            if (_currentState.Name == NarrowStateName)
            {
                MasterVisibility = Visibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
        }

        private void SetGoBack()
        {
            if (SystemNavigationManager.GetForCurrentView() != null)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += ((sender, e) =>
                {
                    if (DetailVisibility == Visibility.Visible)
                    {
                        MasterVisibility = Visibility.Visible;
                        DetailVisibility = Visibility.Collapsed;
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                        e.Handled = true;
                    }
                });
            }
        }
    }
}