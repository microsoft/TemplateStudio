using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
        private readonly ISampleDataService _sampleDataService;
        private ICommand _itemInvokedCommand;
        private ICommand _collapseAllCommand;
        private object _selectedItem;
        private bool _isCollapsed;

        public object SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public bool IsCollapsed
        {
            get => _isCollapsed;
            set
            {
                _isCollapsed = value;
            }
        }

        public ObservableCollection<SampleCompany> SampleItems { get; } = new ObservableCollection<SampleCompany>();

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new DelegateCommand<WinUI.TreeViewItemInvokedEventArgs>(OnItemInvoked));

        public ICommand CollapseAllCommand => _collapseAllCommand ?? (_collapseAllCommand = new DelegateCommand(OnCollapseAll));

        public wts.ItemNameViewModel(ISampleDataService sampleDataServiceInstance)
        {
            _sampleDataService = sampleDataServiceInstance;
        }

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            var data = await _sampleDataService.GetTreeViewDataAsync();
            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            if (SampleItems.Any())
            {
                SelectedItem = SampleItems.First();
            }
        }

        private void OnItemInvoked(WinUI.TreeViewItemInvokedEventArgs args)
            => SelectedItem = args.InvokedItem;

        private void OnCollapseAll()
            => IsCollapsed = true;
    }
}
