using System;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views.MasterDetailsItemDetail
{
    public sealed partial class MasterView
    {
        public MasterView()
        {
            InitializeComponent();
        }

        public MasterDetailsItemDetailViewModel ViewModel => DataContext as MasterDetailsItemDetailViewModel;
    }
}
