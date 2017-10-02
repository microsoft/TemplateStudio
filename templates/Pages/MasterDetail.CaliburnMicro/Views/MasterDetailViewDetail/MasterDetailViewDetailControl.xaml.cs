using System;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views.MasterDetailViewDetail
{
    public sealed partial class MasterDetailViewDetailControl
    {
        public MasterDetailViewDetailControl()
        {
            InitializeComponent();
        }

        public MasterDetailViewDetailViewModel ViewModel => DataContext as MasterDetailViewDetailViewModel;
    }
}
