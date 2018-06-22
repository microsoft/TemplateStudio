using System;

using InkPoc.ViewModels;

using Windows.UI.Xaml.Controls;

namespace InkPoc.Views
{
    public sealed partial class DrawWithControlPage : Page
    {
        public DrawWithControlViewModel ViewModel { get; } = new DrawWithControlViewModel();

        public DrawWithControlPage()
        {
            InitializeComponent();
        }
    }
}
