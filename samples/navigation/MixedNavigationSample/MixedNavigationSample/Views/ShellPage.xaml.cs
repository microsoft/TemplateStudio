using System;

using MixedNavigationSample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);
        }
    }
}
