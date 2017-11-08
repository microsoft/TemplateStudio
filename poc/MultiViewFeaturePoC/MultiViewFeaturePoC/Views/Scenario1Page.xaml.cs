using System;

using MultiViewFeaturePoC.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MultiViewFeaturePoC.Views
{
    public sealed partial class Scenario1Page : Page
    {
        public Scenario1ViewModel ViewModel { get; } = new Scenario1ViewModel();

        public Scenario1Page()
        {
            InitializeComponent();
        }
    }
}
