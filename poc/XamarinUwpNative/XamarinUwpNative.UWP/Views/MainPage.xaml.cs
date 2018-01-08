using System;

using Windows.UI.Xaml.Controls;

using XamarinUwpNative.UWP.ViewModels;

namespace XamarinUwpNative.UWP.Views
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
