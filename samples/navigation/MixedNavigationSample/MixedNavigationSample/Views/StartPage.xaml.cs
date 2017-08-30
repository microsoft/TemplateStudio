using System;

using MixedNavigationSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.Views
{
    public sealed partial class StartPage : Page
    {
        public StartViewModel ViewModel { get; } = new StartViewModel();

        public StartPage()
        {
            InitializeComponent();
        }
    }
}
