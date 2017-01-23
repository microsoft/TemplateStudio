using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Page_NS.App_Page
{
    public class App_PageMasterDetailViewModel : ViewModelBase
    {
        public ICommand LoadDataCommand { get; private set; }
        public ICommand NavigateToDetailCommand { get; private set; }

        public ObservableCollection<DessertModel> DessertList { get; private set; } = new ObservableCollection<DessertModel>();

        private DessertModel _selected;
        public DessertModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public App_PageMasterDetailViewModel()
        {
            LoadDataCommand = new RelayCommand(async () => { await LoadDataAsync(); });
            NavigateToDetailCommand = new RelayCommand<ItemClickEventArgs>(NavigateToDetail);
        }

        public async Task LoadDataAsync()
        {
            DessertList.Clear();

            var service = new DessertService();
            var data = await service.GetDataAsync();

            foreach (var item in data)
            {
                DessertList.Add(item);
            }
        }

        public void NavigateToDetail(ItemClickEventArgs param)
        {
            var item = param.ClickedItem as DessertModel;
            if (item != null)
            {
                this.Selected = item;
            }
        }
    }
}