using System;

using MultiViewFeaturePoC.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MultiViewFeaturePoC.Services;

namespace MultiViewFeaturePoC.Views
{
    public sealed partial class Scenario1SecondaryPage : Page
    {
        public Scenario1SecondaryViewModel ViewModel { get; } = new Scenario1SecondaryViewModel();

        public Scenario1SecondaryPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Initialize(e.Parameter as ViewLifetimeControl);
        }
    }
}
