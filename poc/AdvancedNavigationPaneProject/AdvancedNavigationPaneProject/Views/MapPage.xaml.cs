using System;

using AdvancedNavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AdvancedNavigationPaneProject.Views
{
    public sealed partial class MapPage : Page
    {
        public MapViewModel ViewModel { get; } = new MapViewModel();

        public MapPage()
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
