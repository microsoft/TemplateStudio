using System;

using Windows.UI.Xaml.Controls;

using WtsXamarinUWP.UWP.ViewModels;

namespace WtsXamarinUWP.UWP.Views
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
