using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private ICommand _itemInvokedCommand;
        private ICommand _collapseAllCommand;
        private object _selectedItem;
        private bool _isCollapsed;

        public object SelectedItem
        {
            get => _selectedItem;
            set => Param_Setter(ref _selectedItem, value);
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

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.TreeViewItemInvokedEventArgs>(OnItemInvoked));

        public ICommand CollapseAllCommand => _collapseAllCommand ?? (_collapseAllCommand = new RelayCommand(OnCollapseAll));

        public wts.ItemNameViewModel()
        {
        }

        public async Task LoadDataAsync()
        {
            var data = await SampleDataService.GetTreeViewDataAsync();
            foreach (var item in data)
            {
                SampleItems.Add(item);
            }
        }

        private void OnItemInvoked(WinUI.TreeViewItemInvokedEventArgs args)
            => SelectedItem = args.InvokedItem;

        private void OnCollapseAll()
            => IsCollapsed = true;
    }
}
