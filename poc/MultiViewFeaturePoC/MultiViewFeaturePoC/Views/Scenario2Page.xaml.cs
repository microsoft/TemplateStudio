using System;

using MultiViewFeaturePoC.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MultiViewFeaturePoC.Views
{
    public sealed partial class Scenario2Page : Page
    {
        public Scenario2ViewModel ViewModel { get; } = new Scenario2ViewModel();

        public Scenario2Page()
        {
            InitializeComponent();
        }
    }
}
