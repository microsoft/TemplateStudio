using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ItemNamespace.MasterDetailPage
{
    public class MasterDetailPageViewModel : System.ComponentModel.INotifyPropertyChanged
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
        public ICommand ItemClickCommand { get; private set; }

        public ObservableCollection<DessertModel> DessertList { get; private set; } = new ObservableCollection<DessertModel>();

        public MasterDetailPageViewModel()
        {
            this._useNavigation = Window.Current.Bounds.Width < UseNavigationWithRequested;
            LoadDataCommand = new RelayCommand(async () => { await LoadDataAsync(); });
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
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

        private async Task LoadDataAsync()
        {
            this.DessertList.Clear();

            var service = new DessertService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                this.DessertList.Add(item);
            }
            if (Window.Current.Bounds.Width >= UseNavigationWithRequested)
            {
                Selected = DessertList.First();
                NavigateToDetail();
            }
        }

        private void OnItemClick(ItemClickEventArgs args)
        {
            DessertModel item = args?.ClickedItem as DessertModel;
            if (item != null)
            {
                Selected = item;
                NavigateToDetail();
            }
        }

        private void NavigateToDetail()
        {
            this.DetailVisibility = Visibility.Visible;
            if (this._useNavigation)
            {
                this.MasterVisibility = Visibility.Collapsed;
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
                        this.MasterVisibility = Visibility.Visible;
                        this.DetailVisibility = Visibility.Collapsed;
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                        e.Handled = true;
                    }
                });
            }
        }
    }
}