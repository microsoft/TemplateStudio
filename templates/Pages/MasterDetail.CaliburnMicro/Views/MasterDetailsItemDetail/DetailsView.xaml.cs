using System;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views.MasterDetailsItemDetail
{
    public sealed partial class DetailsView
    {
        public DetailsView()
        {
            InitializeComponent();
        }

        public MasterDetailsItemDetailViewModel ViewModel => DataContext as MasterDetailsItemDetailViewModel;
    }
}
