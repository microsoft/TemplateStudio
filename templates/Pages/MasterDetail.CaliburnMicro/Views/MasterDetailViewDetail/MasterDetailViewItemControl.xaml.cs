using System;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views.MasterDetailViewDetail
{
    public sealed partial class MasterDetailViewItemControl
    {
        public MasterDetailViewItemControl()
        {
            InitializeComponent();
        }

        public MasterDetailViewDetailViewModel ViewModel => DataContext as MasterDetailViewDetailViewModel;
    }
}
