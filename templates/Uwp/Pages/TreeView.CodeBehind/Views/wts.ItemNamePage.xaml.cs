using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;
using Param_RootNamespace.Core.Models;
using Param_RootNamespace.Core.Services;

namespace Param_RootNamespace.Views
{
    // For more info about the TreeView Control see
    // https://docs.microsoft.com/windows/uwp/design/controls-and-patterns/tree-view
    // For other samples, get the XAML Controls Gallery app http://aka.ms/XamlControlsGallery
    public sealed partial class wts.ItemNamePage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private object _selectedItem;

        public object SelectedItem
        {
            get { return _selectedItem; }
            set { Param_Setter(ref _selectedItem, value); }
        }

        public ObservableCollection<SampleCompany> SampleItems { get; } = new ObservableCollection<SampleCompany>();

        public wts.ItemNamePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var data = await SampleDataService.GetTreeViewDataAsync();
            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            if (SampleItems.Any())
            {
                SelectedItem = SampleItems.First();
            }
        }

        private void OnItemInvoked(WinUI.TreeView sender, WinUI.TreeViewItemInvokedEventArgs args)
            => SelectedItem = args.InvokedItem;

        private void OnExpandAll(object sender, RoutedEventArgs e)
            => ExpandOrCollapse(treeView.RootNodes, true);

        private void OnCollapseAll(object sender, RoutedEventArgs e)
            => ExpandOrCollapse(treeView.RootNodes, false);

        private void ExpandOrCollapse(IList<WinUI.TreeViewNode> nodes, bool expand)
        {
            foreach (var node in nodes)
            {
                ExpandOrCollapse(node.Children, expand);

                if (expand)
                {
                    treeView.Expand(node);
                }
                else
                {
                    treeView.Collapse(node);
                }
            }
        }
    }
}
