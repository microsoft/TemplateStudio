using System;

using MultiViewFeaturePoC.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MultiViewFeaturePoC.Views
{
    public sealed partial class Scenario3SecondaryPage : Page
    {
        public Scenario3SecondaryViewModel ViewModel { get; } = new Scenario3SecondaryViewModel();

        public Scenario3SecondaryPage()
        {
            InitializeComponent();
        }
    }
}
