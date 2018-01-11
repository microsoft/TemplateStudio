using System;

using PrismBugRepro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PrismBugRepro.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
