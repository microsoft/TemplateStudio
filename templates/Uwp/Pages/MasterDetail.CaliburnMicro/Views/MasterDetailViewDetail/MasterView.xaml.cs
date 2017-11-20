using System;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views.MasterDetailViewDetail
{
    public sealed partial class MasterView
    {
        public MasterView()
        {
            InitializeComponent();
        }

        public MasterDetailViewDetailViewModel ViewModel => DataContext as MasterDetailViewDetailViewModel;
    }
}
