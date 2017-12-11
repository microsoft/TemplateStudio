using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Windows.Mvvm;
using Prism.Commands;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WTSPrismNavigationBase.Helpers;
using WTSPrismNavigationBase.Views;
using Prism.Windows.Navigation;
using Prism.Events;
using System;
using System.Linq;

namespace WTSPrismNavigationBase.ViewModels
{
    public class ShellPageViewModel : ViewModelBase
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";
        private const double WideStateMinWindowWidth = 640;
        private const double PanoramicStateMinWindowWidth = 1024;
        private readonly INavigationService navigationService;

        public ShellPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            OpenPaneCommand = new DelegateCommand(() => IsPaneOpen = !isPaneOpen);
            ItemSelectedCommand = new DelegateCommand<ItemClickEventArgs>(ItemSelected);
            StateChangedCommand = new DelegateCommand<VisualStateChangedEventArgs>(args => GoToState(args.NewState.Name));
        }

        private bool isPaneOpen;
        public bool IsPaneOpen
        {
            get { return isPaneOpen; }
            set { SetProperty(ref isPaneOpen, value); }
        }

        private SplitViewDisplayMode displayMode = SplitViewDisplayMode.CompactInline;
        public SplitViewDisplayMode DisplayMode
        {
            get { return displayMode; }
            set { SetProperty(ref displayMode, value); }
        }

        private object _lastSelectedItem;
        private object _currentSelectedItem;

        private ObservableCollection<ShellNavigationItem> primaryItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> PrimaryItems
        {
            get { return primaryItems; }
            set { SetProperty(ref primaryItems, value); }
        }

        private ObservableCollection<ShellNavigationItem> secondaryItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> SecondaryItems
        {
            get { return secondaryItems; }
            set { SetProperty(ref secondaryItems, value); }
        }

        public ICommand OpenPaneCommand { get; }
        public ICommand ItemSelectedCommand { get; }
        public ICommand StateChangedCommand { get; }

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
            frame.Navigated += Frame_Navigated;
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
            primaryItems.Clear();
            secondaryItems.Clear();

            // TODO WTS: Change the symbols for each item as appropriate for your app
            // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            // Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/navigationpane.md
            // Edit String/en-US/Resources.resw: Add a menu item title for each page
            primaryItems.Add(new ShellNavigationItem("Shell_Main".GetLocalized(), Symbol.Document, "Main"));
            primaryItems.Add(new ShellNavigationItem("Shell_ChartPage".GetLocalized(), Symbol.Document, "Chart"));
            primaryItems.Add(new ShellNavigationItem("Shell_GridPage".GetLocalized(), Symbol.Document, "Grid"));
            primaryItems.Add(new ShellNavigationItem("Shell_MapPage".GetLocalized(), Symbol.Document, "Map"));
            primaryItems.Add(new ShellNavigationItem("Shell_MediaPlayerPage".GetLocalized(), Symbol.Document, "MediaPlayer"));
            primaryItems.Add(new ShellNavigationItem("Shell_SettingsPage".GetLocalized(), Symbol.Document, "Settings"));
            primaryItems.Add(new ShellNavigationItem("Shell_TabbedPage".GetLocalized(), Symbol.Document, "Tabbed"));
            primaryItems.Add(new ShellNavigationItem("Shell_WebViewPage".GetLocalized(), Symbol.Document, "WebView"));
            primaryItems.Add(new ShellNavigationItem("Shell_MasterDetail".GetLocalized(), Symbol.Document, "MasterDetail"));
        }

        private void ItemSelected(ItemClickEventArgs args)
        {
            if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
            {
                IsPaneOpen = false;
            }
            Navigate(args.ClickedItem);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e != null)
            {
                var vm = e.SourcePageType.ToString().Split('.').Last().Replace("Page",String.Empty);
                var navigationItem = PrimaryItems?.FirstOrDefault(i => i.PageIdentifier == vm);
                if (navigationItem == null)
                {
                    navigationItem = SecondaryItems?.FirstOrDefault(i => i.PageIdentifier == vm);
                }

                if (navigationItem != null)
                {
                    ChangeSelected(_lastSelectedItem, navigationItem);
                    _lastSelectedItem = navigationItem;
                }
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
                navigationService.Navigate(navigationItem.PageIdentifier, null);
            }
        }
    }
}
