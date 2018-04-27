using System;

using AdvancedNavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AdvancedNavigationPaneProject.Views
{
    public sealed partial class SecondShellPage : Page
    {
        public SecondShellViewModel ViewModel { get; } = new SecondShellViewModel();

        public SecondShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(shellFrame, navigationView);
        }
    }
}
