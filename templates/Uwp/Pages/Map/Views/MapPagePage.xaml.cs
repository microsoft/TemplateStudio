using System;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    public sealed partial class MapPagePage : Page
    {
        public MapPagePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.InitializeAsync(mapControl);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Cleanup();
        }
    }
}
