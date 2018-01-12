using System;

using Windows.UI.Xaml.Controls;

using Wts.UWP.ViewModels;

namespace Wts.UWP.Views
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
