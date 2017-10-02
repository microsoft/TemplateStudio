using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Param_ItemNamespace.Views
{
    public sealed partial class MasterDetailViewPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private SampleOrder _selected;

        public SampleOrder Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        public MasterDetailViewPage()
        {
            InitializeComponent();
            Loaded += MasterDetailViewPage_Loaded;
        }

        private async void MasterDetailViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            SampleItems.Clear();

            var data = await SampleDataService.GetSampleModelDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            if (WindowStates.CurrentState.Name == "WideState")
            {
                Selected = SampleItems.First();
            }
        }
    }
}
