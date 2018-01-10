using System;

using Windows.UI.Xaml.Controls;

using WtsXPlat.UWP.ViewModels;

namespace WtsXPlat.UWP.Views
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
