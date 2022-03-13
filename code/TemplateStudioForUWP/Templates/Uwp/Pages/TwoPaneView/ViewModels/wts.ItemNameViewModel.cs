using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private WinUI.TwoPaneView _twoPaneView;
        private SampleOrder _selected;
        private ICommand _itemClickCommand;
        private ICommand _modeChangedCommand;

        private WinUI.TwoPaneViewPriority _twoPanePriority;

        public SampleOrder Selected
        {
            get { return _selected; }
            set { Param_Setter(ref _selected, value); }
        }

        public WinUI.TwoPaneViewPriority TwoPanePriority
        {
            get { return _twoPanePriority; }
            set { Param_Setter(ref _twoPanePriority, value); }
        }

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand(OnItemClick));

        public ICommand ModeChangedCommand => _modeChangedCommand ?? (_modeChangedCommand = new RelayCommand<WinUI.TwoPaneView>(OnModeChanged));

        public wts.ItemNameViewModel()
        {
        }

        public void Initialize(WinUI.TwoPaneView twoPaneView)
        {
            _twoPaneView = twoPaneView;
        }

        public async Task LoadDataAsync()
        {
            SampleItems.Clear();

            var data = await SampleDataService.GetTwoPaneViewDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            Selected = SampleItems.First();
        }

        public bool TryCloseDetail()
        {
            if (TwoPanePriority == WinUI.TwoPaneViewPriority.Pane2)
            {
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1;
                return true;
            }

            return false;
        }

        private void OnItemClick()
        {
            if (_twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2;
            }
        }

        private void OnModeChanged(WinUI.TwoPaneView twoPaneView)
        {
            if (twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2;
            }
            else
            {
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1;
            }
        }
    }
}
