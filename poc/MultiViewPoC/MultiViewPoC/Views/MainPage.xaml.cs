using System;

using MultiViewPoC.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MultiViewPoC.Views
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
