using System;

using Windows.UI.Xaml.Controls;

using WTSGeneratedNavigation.ViewModels;

namespace WTSGeneratedNavigation.Views
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
