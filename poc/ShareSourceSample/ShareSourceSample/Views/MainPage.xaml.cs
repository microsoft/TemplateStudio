using System;

using ShareSourceSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace ShareSourceSample.Views
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
