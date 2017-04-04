using Windows.UI.Xaml.Controls;
using ItemNamespace.Helper;
using ItemNamespace.Models;
using ItemNamespace.Services;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace ItemNamespace.Views
{
    public sealed partial class MasterDetailPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private SampleModel _selected;
        public SampleModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand ItemClickCommand { get; private set; }
 
        public ObservableCollection<SampleModel> SampleItems { get; private set; } = new ObservableCollection<SampleModel>();  

        public MasterDetailPage()
        {
            this.InitializeComponent();
        }

        private void Initialize()
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
        }

        private async Task LoadDataAsync() 
        {  
            SampleItems.Clear(); 

            var service = new SampleModelService(); 
            var data = await service.GetDataAsync(); 

            foreach (var item in data) 
            { 
                SampleItems.Add(item); 
            } 
            Selected = SampleItems.First(); 
        } 


        private void OnItemClick(ItemClickEventArgs args)
        {
            SampleModel item = args?.ClickedItem as SampleModel;
            if (item != null)
            {
                if (WindowStates.CurrentState == NarrowState)
                {
                    NavigationService.Navigate<Views.MasterDetailDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
            }
        }
    }
}
