using System;

using MultiViewPoC.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MultiViewPoC.Views
{
    public sealed partial class ImagesPage : Page
    {
        public ImagesViewModel ViewModel { get; } = new ImagesViewModel();

        public ImagesPage()
        {
            InitializeComponent();
        }

        private async void GridView_Loaded(object sender, RoutedEventArgs e)
        {
            var gridView = sender as GridView;
            await ViewModel.LoadAnimationAsync(gridView);
        }
    }
}
