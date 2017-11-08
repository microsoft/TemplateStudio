using System;

using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DragAndDropExample.Views
{
    public sealed partial class Scenario4Page : Page
    {
        public Scenario4ViewModel ViewModel { get; } = new Scenario4ViewModel();

        public Scenario4Page()
        {
            InitializeComponent();
        }


        private void ListView_DragOver(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            DragMask.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void ListView_DragLeave(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            DragMask.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void ListView_Drop(object sender, Windows.UI.Xaml.DragEventArgs e)
        {
            DragMask.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
