using uct.ItemName.Services;
using uct.ItemName.Helper;
using uct.ItemName.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace uct.ItemName.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {     
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

        public ShellPage()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            NavigationService.Frame = shellFrame;
            NavigationService.Frame.Navigated += NavigationService_Navigated;            
            PopulateNavItems();
        }

        private void PopulateNavItems()
        {
            _primaryItems.Clear();
            _secondaryItems.Clear();

            // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            // Edit String/en-US/Resources.resw: Add a menu item title for each page
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.NewState == PanoramicState)
            {
                DisplayMode = SplitViewDisplayMode.CompactInline;
            }
            else if (args.NewState == WideState)
            {
                DisplayMode = SplitViewDisplayMode.CompactInline;
                IsPaneOpen = false;
            }
            else if (args.NewState == NarrowState)
            {
                DisplayMode = SplitViewDisplayMode.Overlay;
                IsPaneOpen = false;
            }
        }

        private void OnPrimaryListViewSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Any() && secondaryListView != null)
            {
                if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
                {
                    IsPaneOpen = false;
                }

                secondaryListView.SelectedIndex = -1;
                secondaryListView.SelectedItem = null;

                // Navigate to selected item
                Navigate(primaryListView.SelectedItem);
            }
        }

        private void OnSecondaryListViewSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Any() && primaryListView != null)
            {
                if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
                {
                    IsPaneOpen = false;
                }

                primaryListView.SelectedIndex = -1;
                primaryListView.SelectedItem = null;

                // Navigate to selected item
                Navigate(secondaryListView.SelectedItem);
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
