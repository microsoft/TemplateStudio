using System;

using AdvancedNavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AdvancedNavigationPaneProject.Views
{
    public sealed partial class StartUpPage : Page
    {
        public StartUpViewModel ViewModel { get; } = new StartUpViewModel();

        public StartUpPage()
        {
            InitializeComponent();
        }
    }
}
