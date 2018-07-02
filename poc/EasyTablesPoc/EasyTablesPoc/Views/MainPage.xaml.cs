using System;

using EasyTablesPoc.ViewModels;

using Windows.UI.Xaml.Controls;

namespace EasyTablesPoc.Views
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
