using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSGeneratedNavigation.ViewModels;

namespace WTSGeneratedNavigation.Views
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
