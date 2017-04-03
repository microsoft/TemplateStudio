using Windows.UI.Xaml;
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
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private VisualState _currentState;

        private SampleModel _selected;
        public SampleModel Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
        
        public ICommand ItemClickCommand { get; private set; }

        public ObservableCollection<SampleModel> SampleItems { get; private set; } = new ObservableCollection<SampleModel>();  

        public async Task LoadDataAsync()
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
                if (_currentState.Name == NarrowStateName)
                {
                    NavigationService.Navigate<Views.MasterDetailDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
            }            
        }  

        public MasterDetailPage()
        {
            this.InitializeComponent();
            
        }

        private void Initialize(VisualState currentState)
        {
            ItemClickCommand = new RelayCommand<ItemClickEventArgs>(OnItemClick);
            _currentState = currentState;
        }

        private void OnAdaptiveStatesCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            _currentState = e.NewState;
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
