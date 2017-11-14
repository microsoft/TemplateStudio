using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WtsXamarin.Helpers;
using WtsXamarin.Models;
using WtsXamarin.Services;
using WtsXamarin.Views;
using Xamarin.Forms;

namespace WtsXamarin.ViewModels
{
    public class ListViewMasterViewModel : Observable
    {
        private ObservableCollection<SampleOrder> _sampleData;

        public ListViewMasterViewModel()
        {
            SampleData = SampleDataService.GetGridSampleData();
        }

        public ObservableCollection<SampleOrder> SampleData
        {
            get => _sampleData;
            private set => Set(ref _sampleData, value);
        }

        public SampleOrder SelectedItem
        {
            get => null;
            set => NavigationService.Instance.NavigateTo<ListViewDetailViewModel>(value);
        }
    }
}
