using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Contracts.Views;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Views
{
    public partial class ts.ItemNamePage : Page, INotifyPropertyChanged, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly ISampleDataService _sampleDataService;

        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public ts.ItemNamePage(INavigationService navigationService, ISampleDataService sampleDataService)
        {
            _navigationService = navigationService;
            _sampleDataService = sampleDataService;
            InitializeComponent();
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await _sampleDataService.GetContentGridDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom()
        {
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            => SelectItem(e);

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SelectItem(e);
                e.Handled = true;
            }
        }

        private void SelectItem(RoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement selectedItem
                && selectedItem.DataContext is SampleOrder order)
            {
                _navigationService.NavigateTo(typeof(ts.ItemNameDetailPage), order.OrderID);
            }
        }
    }
}
