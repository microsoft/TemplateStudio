using System;

using AdvancedNavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AdvancedNavigationPaneProject.Views
{
    public sealed partial class SecondShellPage : Page
    {
        public SecondShellViewModel ViewModel { get; } = new SecondShellViewModel();

        public SecondShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView);
        }
    }
}
