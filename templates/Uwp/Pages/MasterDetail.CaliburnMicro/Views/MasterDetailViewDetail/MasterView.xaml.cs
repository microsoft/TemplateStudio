using System;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Views.MasterDetailViewDetail
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
