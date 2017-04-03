using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ItemNamespace.Models;
using ItemNamespace.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ItemNamespace.Views
{
    public sealed partial class MasterDetailDetailPage : Page, INotifyPropertyChanged
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private SampleModel _item;
        public SampleModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public MasterDetailDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Item = e.Parameter as SampleModel;
        }

        private void OnAdaptiveStatesCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            if (e.OldState.Name == NarrowStateName && e.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
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
