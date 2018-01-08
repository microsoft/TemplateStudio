using System;

using Windows.UI.Xaml.Controls;

using XamarinUwpNative.UWP.ViewModels;

namespace XamarinUwpNative.UWP.Views
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
