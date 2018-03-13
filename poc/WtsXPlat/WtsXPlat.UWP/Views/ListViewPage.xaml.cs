using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using WtsXPlat.UWP.ViewModels;

namespace WtsXPlat.UWP.Views
{
    public sealed partial class ListViewPage : Page
    {
        public ListViewViewModel ViewModel { get; } = new ListViewViewModel();

        public ListViewPage()
        {
            InitializeComponent();
            Loaded += ListViewPage_Loaded;
        }

        private async void ListViewPage_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }
    }
}
