using System;
using AdvancedNavigationPaneProject.Services;
using AdvancedNavigationPaneProject.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AdvancedNavigationPaneProject.Views
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
