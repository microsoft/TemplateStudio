using System;

using WebToAppLink.ViewModels;

using Windows.UI.Xaml.Controls;

namespace WebToAppLink.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
