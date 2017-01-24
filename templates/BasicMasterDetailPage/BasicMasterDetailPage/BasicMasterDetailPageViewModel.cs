using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Page_NS.Core;

namespace Page_NS.BasicMasterDetailPage
{
    public class BasicMasterDetailPageViewModel : ViewModelBase
    {
        const double UseNavigationWithRequested = 900;

        private bool _useNavigation;

        private Visibility _masterVisibility;
        public Visibility MasterVisibility
        {
            get => _masterVisibility;
            set => Set(ref _masterVisibility, value);
        }

        private Visibility _detailVisibility = Visibility.Collapsed;
        public Visibility DetailVisibility
        {
            get => _detailVisibility;
            set => Set(ref _detailVisibility, value);
        }

        private DessertModel _selected;
        public DessertModel Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        public ICommand LoadDataCommand { get; private set; }
        public ICommand NavigateToDetailCommand { get; private set; }

        public ObservableCollection<DessertModel> DessertList { get; private set; } = new ObservableCollection<DessertModel>();

        public BasicMasterDetailPageViewModel()
        {
            this._useNavigation = Window.Current.Bounds.Width < UseNavigationWithRequested;
            LoadDataCommand = new RelayCommand(async () => { await LoadDataAsync(); });
            NavigateToDetailCommand = new RelayCommand<ItemClickEventArgs>(NavigateToDetail);
            Window.Current.SizeChanged += OnWindowSizeChanged;
            SetGoBack();
        }

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var newWidth = e.Size.Width;
            if (!this._useNavigation && newWidth < UseNavigationWithRequested)
            {
                //Enter on navigation master detail
                this.MasterVisibility = Visibility.Visible;
                this.DetailVisibility = Visibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            else if (this._useNavigation && newWidth >= UseNavigationWithRequested)
            {
                //Enter on full screen master detail
                this.MasterVisibility = Visibility.Visible;
                this.DetailVisibility = Visibility.Visible;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            this._useNavigation = newWidth < UseNavigationWithRequested;
        }

        public async Task LoadDataAsync()
        {
            this.DessertList.Clear();

            var service = new DessertService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                this.DessertList.Add(item);
            }
        }

        public void NavigateToDetail(ItemClickEventArgs param)
        {
            var item = param.ClickedItem as DessertModel;
            if (item != null)
            {
                this.Selected = item;
                this.DetailVisibility = Visibility.Visible;
                if (this._useNavigation)
                {
                    this.MasterVisibility = Visibility.Collapsed;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }
            }
        }

        private void SetGoBack()
        {
            if (SystemNavigationManager.GetForCurrentView() != null)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += ((sender, e) =>
                {
                    this.MasterVisibility = Visibility.Visible;
                    this.DetailVisibility = Visibility.Collapsed;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                });
            }
        }
    }
}