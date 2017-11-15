using System;

using MultiViewFeaturePoC.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MultiViewFeaturePoC.Views
{
    public sealed partial class Scenario1SecondaryPage : Page
    {
        public Scenario1SecondaryViewModel ViewModel { get; } = new Scenario1SecondaryViewModel();

        public Scenario1SecondaryPage()
        {
            InitializeComponent();
        }
    }
}
