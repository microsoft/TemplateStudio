using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using uct.ItemName.Models;
using uct.ItemName.Services;
using uct.ItemName.Views;
using uct.ItemName.Helpers;

namespace uct.ItemName.ViewModels
{
    public class ShellViewModel : Observable
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";

        private ListView _primaryListView;
        private ListView _secondaryListView;

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private SplitViewDisplayMode _displayMode = SplitViewDisplayMode.CompactInline;
        public SplitViewDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set { Set(ref _displayMode, value); }
        }

        private object _primarySelectedItem;
        public object PrimarySelectedItem
        {
            get { return _primarySelectedItem; }
            set
            {
                ChangeSelected(_primarySelectedItem, value);
                Set(ref _primarySelectedItem, value);
            }
        }

        private object _secondarySelectedItem;
        public object SecondarySelectedItem
        {
            get { return _secondarySelectedItem; }
            set
            {
                ChangeSelected(_secondarySelectedItem, value);
                Set(ref _secondarySelectedItem, value);
            }
        }

        private ObservableCollection<ShellNavigationItem> _primaryItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> PrimaryItems
        {
            get { return _primaryItems; }
            set { Set(ref _primaryItems, value); }
        }

        private ObservableCollection<ShellNavigationItem> _secondaryItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> SecondaryItems
        {
            get { return _secondaryItems; }
            set { Set(ref _secondaryItems, value); }
        }

        private ICommand _openPaneCommand;
        public ICommand OpenPaneCommand
        {
            get
            {
                if (_openPaneCommand == null)
                {
                    _openPaneCommand = new RelayCommand(() => IsPaneOpen = !_isPaneOpen);
                }

                return _openPaneCommand;
            }
        }

        private ICommand _primaryListViewSelectionChangedCommand;
        public ICommand PrimaryListViewSelectionChangedCommand
        {
            get
            {
                if (_primaryListViewSelectionChangedCommand == null)
                {
                    _primaryListViewSelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnPrimaryListViewSelectionChanged);
                }

                return _primaryListViewSelectionChangedCommand;
            }
        }

        private ICommand _secondaryListViewSelectionChangedCommand;
        public ICommand SecondaryListViewSelectionChangedCommand
        {
            get
            {
                if (_secondaryListViewSelectionChangedCommand == null)
                {
                    _secondaryListViewSelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnSecondaryListViewSelectionChanged);
                }

                return _secondaryListViewSelectionChangedCommand;
            }
        }

        private ICommand _stateChangedCommand;
        public ICommand StateChangedCommand
        {
            get
            {
                if (_stateChangedCommand == null)
                {
                    _stateChangedCommand = new RelayCommand<Windows.UI.Xaml.VisualStateChangedEventArgs>(OnStateChanged);
                }

                return _stateChangedCommand;
            }
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.NewState.Name == PanoramicStateName)
            {
                DisplayMode = SplitViewDisplayMode.CompactInline;
            }
            else if (args.NewState.Name == WideStateName)
            {
                DisplayMode = SplitViewDisplayMode.CompactInline;
                IsPaneOpen = false;
            }
            else if (args.NewState.Name == NarrowStateName)
            {
                DisplayMode = SplitViewDisplayMode.Overlay;
                IsPaneOpen = false;
            }

        }

        public void Initialize(Frame frame, ListView primaryListView, ListView secondaryListView)
        {
            _primaryListView = primaryListView;
            _secondaryListView = secondaryListView;
            NavigationService.Frame = frame;
            NavigationService.Frame.Navigated += NavigationService_Navigated;            
            PopulateNavItems();
        }

        private void PopulateNavItems()
        {
            _primaryItems.Clear();
            _secondaryItems.Clear();

            //More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            //Edit String/en-US/Resources.resw: Add a menu item title for each page
        }

        private void OnPrimaryListViewSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Any() && _secondaryListView != null)
            {
                if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
                {
                    IsPaneOpen = false;
                }

                _secondaryListView.SelectedIndex = -1;
                _secondaryListView.SelectedItem = null;

                //Navigate to selected item
                Navigate(_primaryListView.SelectedItem);
            }
        }

        private void OnSecondaryListViewSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Any() && _primaryListView != null)
            {
                if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
                {
                    IsPaneOpen = false;
                }

                _primaryListView.SelectedIndex = -1;
                _primaryListView.SelectedItem = null;

                //Navigate to selected item
                Navigate(_secondaryListView.SelectedItem);
            }
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            var item = PrimaryItems?.FirstOrDefault(i => i.PageType == e?.SourcePageType);
            if (item != null)
            {
                PrimarySelectedItem = item;
            }
        }        

        private void ChangeSelected(object oldValue, object newValue)
        {
            if (oldValue != null)
            {
                (oldValue as ShellNavigationItem).IsSelected = false;
            }
            if (newValue != null)
            {
                (newValue as ShellNavigationItem).IsSelected = true;
            }
        }

        private void Navigate(object item)
        {
            var navigationItem = item as ShellNavigationItem;
            if (navigationItem != null)
            {
                NavigationService.Navigate(navigationItem.PageType);
            }
        }
    }
}
        