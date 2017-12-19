using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Param_ItemNamespace.ViewModels;

namespace Param_ItemNamespace.Views
{
    public sealed partial class MasterDetailDetailPage : Page
    {
        private MasterDetailDetailViewModel ViewModel => DataContext as MasterDetailDetailViewModel;

        public MasterDetailDetailPage()
        {
            InitializeComponent();
        }
    }
}
