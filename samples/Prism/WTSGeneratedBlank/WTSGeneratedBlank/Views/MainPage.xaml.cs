using System;

using Windows.UI.Xaml.Controls;

using WTSGeneratedBlank.ViewModels;

namespace WTSGeneratedBlank.Views
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
