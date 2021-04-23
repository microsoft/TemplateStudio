using System;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using WinUIDesktopApp.Contracts.ViewModels;
using WinUIDesktopApp.Core.Contracts.Services;
using WinUIDesktopApp.Core.Models;

namespace WinUIDesktopApp.ViewModels
{
    public class ListDetailsViewModel : ObservableRecipient, INavigationAware
    {
        private readonly ISampleDataService _sampleDataService;
        private SampleOrder _selected;

        public SampleOrder Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ObservableCollection<SampleOrder> SampleItems { get; private set; } = new ObservableCollection<SampleOrder>();

        public ListDetailsViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            SampleItems.Clear();

            // Replace this with your actual data
            var data = await _sampleDataService.GetListDetailsDataAsync();

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
