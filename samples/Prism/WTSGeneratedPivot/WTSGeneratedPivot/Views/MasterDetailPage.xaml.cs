using System;

using Windows.UI.Xaml.Controls;

using WTSGeneratedPivot.ViewModels;

namespace WTSGeneratedPivot.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        private MasterDetailViewModel ViewModel => DataContext as MasterDetailViewModel;

        public MasterDetailPage()
        {
            InitializeComponent();
        }
    }
}
