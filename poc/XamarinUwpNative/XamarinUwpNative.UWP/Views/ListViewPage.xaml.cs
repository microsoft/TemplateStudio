using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using XamarinUwpNative.UWP.ViewModels;

namespace XamarinUwpNative.UWP.Views
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
