using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using ItemNamespace.Models;
using ItemNamespace.Services;

namespace ItemNamespace.ViewModels
{
    public class MasterDetailDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        public ICommand StateChangedCommand { get; private set; }

        private SampleModel _item;
        public SampleModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public MasterDetailDetailViewModel()
        {
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }
        
        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }
    }
}
