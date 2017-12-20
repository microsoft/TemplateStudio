using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Caliburn.Micro;
using wts.ItemName.Services;
using wts.ItemName.Views;

namespace wts.ItemName.ViewModels
{
    public class ShellViewModel : Screen
    {
        private const string PanoramicStateName = "PanoramicState";
        private const string WideStateName = "WideState";
        private const string NarrowStateName = "NarrowState";
        private const double WideStateMinWindowWidth = 640;
        private const double PanoramicStateMinWindowWidth = 1024;

        private readonly WinRTContainer _container;
        private INavigationService _navigationService;

        public ShellViewModel(WinRTContainer container)
        {
            _container = container;
        }

        private bool _isPaneOpen;

        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            set { Set(ref _isPaneOpen, value); }
        }

        private object _selected;

        public object Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private SplitViewDisplayMode _displayMode = SplitViewDisplayMode.CompactInline;

        public SplitViewDisplayMode DisplayMode
        {
            get { return _displayMode; }
            set { Set(ref _displayMode, value); }
        }

        private object _lastSelectedItem;

        public ObservableCollection<ShellNavigationItem> PrimaryItems { get; } = new ObservableCollection<ShellNavigationItem>();

        public ObservableCollection<ShellNavigationItem> SecondaryItems { get; } = new ObservableCollection<ShellNavigationItem>();

        public void Open()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        public void StateChanged(VisualStateChangedEventArgs args)
        {
            GoToState(args.NewState.Name);
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

        protected override void OnInitialize()
        {
            var view = GetView() as IShellView;

            _navigationService = view?.CreateNavigationService(_container);

            if (_navigationService != null)
            {
                _navigationService.Navigated += NavigationService_Navigated;
            }

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
            PrimaryItems.Clear();
            SecondaryItems.Clear();

            // TODO WTS: Change the symbols for each item as appropriate for your app
            // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            // Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/navigationpane.md
            // Edit String/en-US/Resources.resw: Add a menu item title for each page
        }

        public void ItemSelected(HamburgetMenuItemInvokedEventArgs args)
        {
            if (DisplayMode == SplitViewDisplayMode.CompactOverlay || DisplayMode == SplitViewDisplayMode.Overlay)
            {
                IsPaneOpen = false;
            }

            Navigate(args.InvokedItem);
        }

        private void NavigationService_Navigated(object sender, NavigationEventArgs e)
        {
            if (e != null)
            {
                var viewType = _navigationService.CurrentSourcePageType;
                var viewModelType = ViewModelLocator.LocateTypeForViewType(viewType, false);

                var navigationItem = PrimaryItems?.FirstOrDefault(i => i.ViewModelType == viewModelType);
                if (navigationItem == null)
                {
                    navigationItem = SecondaryItems?.FirstOrDefault(i => i.ViewModelType == viewModelType);
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
                Selected = newValue;
            }
        }

        private void Navigate(object item)
        {
            if (_lastSelectedItem == item)
            {
                return;
            }

            var navigationItem = item as ShellNavigationItem;
            if (navigationItem != null)
            {
                _navigationService.NavigateToViewModel(navigationItem.ViewModelType);
            }
        }
    }
}
