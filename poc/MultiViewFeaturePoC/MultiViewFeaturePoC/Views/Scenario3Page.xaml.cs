using System;

using MultiViewFeaturePoC.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MultiViewFeaturePoC.Views
{
    public sealed partial class Scenario3Page : Page
    {
        public Scenario3ViewModel ViewModel { get; } = new Scenario3ViewModel();

        public Scenario3Page()
        {
            InitializeComponent();
        }
    }
}
