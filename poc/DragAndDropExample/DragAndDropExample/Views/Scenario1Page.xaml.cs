using System;

using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;

namespace DragAndDropExample.Views
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
