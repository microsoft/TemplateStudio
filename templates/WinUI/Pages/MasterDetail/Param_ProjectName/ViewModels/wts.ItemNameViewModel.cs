using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Mvvm.ComponentModel;

using Param_RootNamespace.Contracts.ViewModels;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;
        private SampleOrder _selected;

        public SampleOrder Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        public wts.ItemNameViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            SampleItems.Clear();

            var data = await _sampleDataService.GetMasterDetailDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }
        }

        public void OnNavigatedFrom()
        {
        }

        public void EnsureItemSelected()
        {
            if (Selected == null)
            {
                Selected = SampleItems.First();
            }
        }
    }
}
