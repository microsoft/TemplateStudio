using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;
using ItemNamespace.Helper;
using ItemNamespace.Models;
using ItemNamespace.Services;

namespace ItemNamespace.Views
{
    public sealed partial class MasterDetailDetailPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private SampleModel _item;
        public SampleModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public ICommand StateChangedCommand { get; private set; }

        public MasterDetailDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Initialize();
            Item = e.Parameter as SampleModel;
        }

        private void Initialize()
        {
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }

        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState == NarrowState && args.NewState == WideState)
            {
                NavigationService.GoBack();
            }
        }
    }
}
