using Windows.UI.Xaml.Controls;
using ItemNamespace.Helper;
using ItemNamespace.Models;
using ItemNamespace.Services;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace ItemNamespace.Views
{
    public sealed partial class MasterDetailPage : Page, INotifyPropertyChanged
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

        private void OnItemClick(ItemClickEventArgs args)
        {
            SampleModel item = args?.ClickedItem as SampleModel;
            if (item != null)
            {
                if (AdaptiveStates.CurrentState == NarrowState)
                {
                    NavigationService.Navigate<Views.MasterDetailDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
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
