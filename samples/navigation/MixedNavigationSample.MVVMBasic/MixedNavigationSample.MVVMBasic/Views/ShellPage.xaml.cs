using System;

using MixedNavigationSample.MVVMBasic.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.MVVMBasic.Views
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
