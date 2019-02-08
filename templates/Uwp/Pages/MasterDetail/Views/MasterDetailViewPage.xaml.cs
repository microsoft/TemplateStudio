using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    public sealed partial class MasterDetailViewPage : Page
    {
        public MasterDetailViewPage()
        {
            InitializeComponent();
            Loaded += MasterDetailViewPage_Loaded;
        }

        private async void MasterDetailViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
