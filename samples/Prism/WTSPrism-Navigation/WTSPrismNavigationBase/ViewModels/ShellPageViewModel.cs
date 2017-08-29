using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Prism.Windows.Mvvm;
using Prism.Commands;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSPrismNavigationBase.Helpers;
using WTSPrismNavigationBase.Services;
using WTSPrismNavigationBase.Views;
using Prism.Windows.Navigation;
using WTSPrismNavigationBase.Interfaces;

namespace WTSPrismNavigationBase.ViewModels
{
    public class ShellPageViewModel : ViewModelBase
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";
        private const double WideStateMinWindowWidth = 640;
        private const double PanoramicStateMinWindowWidth = 1024;
        private INavigationService _navService;

        public ShellPageViewModel(INavigationService navigationService):base()
        {
            _navService = navigationService;
        }

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { SetProperty(ref _isPaneOpen, value); }
        }

        private SplitViewDisplayMode _displayMode = SplitViewDisplayMode.CompactInline;
        public SplitViewDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set { SetProperty(ref _displayMode, value); }
        }

        private object _lastSelectedItem;
        private object _currentSelectedItem;

        private ObservableCollection<ShellNavigationItem> _primaryItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> PrimaryItems
        {
            get { return _primaryItems; }
            set { SetProperty(ref _primaryItems, value); }
        }

        private ObservableCollection<ShellNavigationItem> _secondaryItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> SecondaryItems
        {
            get { return _secondaryItems; }
            set { SetProperty(ref _secondaryItems, value); }
        }

        private ICommand _openPaneCommand;
        public ICommand OpenPaneCommand
        {
            get
            {
                if (_openPaneCommand == null)
                {
                    _openPaneCommand = new DelegateCommand(() => IsPaneOpen = !_isPaneOpen);
                }

                return _openPaneCommand;
            }
        }

        private ICommand _itemSelected;
        public ICommand ItemSelectedCommand
        {
            get
            {
                if (_itemSelected == null)
                {
                    _itemSelected = new DelegateCommand<ItemClickEventArgs>(ItemSelected);
                }

                return _itemSelected;
            }
        }

        private ICommand _stateChangedCommand;
        public ICommand StateChangedCommand
        {
            get
            {
                if (_stateChangedCommand == null)
                {
                    _stateChangedCommand = new DelegateCommand<Windows.UI.Xaml.VisualStateChangedEventArgs>(args => GoToState(args.NewState.Name));
                }

                return _stateChangedCommand;
            }
        }

        private void GoToState(string stateName)
        {
            switch (stateName)
            {
                case PanoramicStateName:
                    DisplayMode = SplitViewDisplayMode.CompactInline;
                    break;
                case WideStateName:
                    DisplayMode = SplitViewDisplayMode.CompactInline;
                    IsPaneOpen = false;
                    break;
                case NarrowStateName:
                    DisplayMode = SplitViewDisplayMode.Overlay;
                    IsPaneOpen = false;
                    break;
                default:
                    break;
            }
        }

        public void Initialize(Frame frame)
        {
            frame.Navigated += NavigationService_Navigated;                       
            PopulateNavItems();
            InitializeState(Window.Current.Bounds.Width);
        }

        private void InitializeState(double windowWith)
        {
            if (windowWith < WideStateMinWindowWidth)
            {
                GoToState(NarrowStateName);
            }
            else if (windowWith < PanoramicStateMinWindowWidth)
            {
                GoToState(WideStateName);
            }
            else
            {
                GoToState(PanoramicStateName);
            }
        }

        private void PopulateNavItems()
        {
            _primaryItems.Clear();
            _secondaryItems.Clear();

            // TODO WTS: Change the symbols for each item as appropriate for your app
            // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            // Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/navigationpane.md
            // Edit String/en-US/Resources.resw: Add a menu item title for each page
            _primaryItems.Add(new ShellNavigationItem("Shell_Main".GetLocalized(), Symbol.Document, "Main"));
            _primaryItems.Add(new ShellNavigationItem("Shell_ChartPage".GetLocalized(), Symbol.Document, "Chart"));
            _primaryItems.Add(new ShellNavigationItem("Shell_GridPage".GetLocalized(), Symbol.Document, "Grid"));
            _primaryItems.Add(new ShellNavigationItem("Shell_MapPage".GetLocalized(), Symbol.Document, "Map"));
            _primaryItems.Add(new ShellNavigationItem("Shell_MediaPlayerPage".GetLocalized(), Symbol.Document, "MediaPlayer"));
            _primaryItems.Add(new ShellNavigationItem("Shell_SettingsPage".GetLocalized(), Symbol.Document, "Settings"));
            _primaryItems.Add(new ShellNavigationItem("Shell_TabbedPage".GetLocalized(), Symbol.Document, "Tabbed"));
            _primaryItems.Add(new ShellNavigationItem("Shell_WebViewPage".GetLocalized(), Symbol.Document, "WebView"));
            _primaryItems.Add(new ShellNavigationItem("Shell_MasterDetail".GetLocalized(), Symbol.Document, "MasterDetail"));
        }

        private void ItemSelected(ItemClickEventArgs args)
        {
            if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
            {
                IsPaneOpen = false;
            }
            _lastSelectedItem = _currentSelectedItem;
            _currentSelectedItem = args.ClickedItem;
            Navigate(args.ClickedItem);
            
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            if (e != null)
            {

                ChangeSelected(_lastSelectedItem, _currentSelectedItem);
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
                _navService.Navigate(navigationItem.PageIdentifier, null);
            }
        }
    }
}
