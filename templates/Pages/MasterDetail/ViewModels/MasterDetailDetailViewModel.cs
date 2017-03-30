using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ItemNamespace.Models;
using ItemNamespace.Services;

namespace ItemNamespace.ViewModels
{
    public class MasterDetailDetailViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        private SampleModel _item;
        public SampleModel Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public MasterDetailDetailViewModel()
        {
        }

        public void UpdateWindowState(VisualStateChangedEventArgs e)
        {
            if (e.OldState.Name == NarrowStateName && e.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }
    }
}