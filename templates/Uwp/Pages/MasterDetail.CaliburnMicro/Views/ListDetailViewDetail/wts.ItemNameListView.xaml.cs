using System;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views.ListDetailViewDetail
{
    public sealed partial class wts.ItemNameListView
    {
        public wts.ItemNameListView()
        {
            InitializeComponent();
        }

        public wts.ItemNameDetailViewModel ViewModel => DataContext as wts.ItemNameDetailViewModel;
    }
}
