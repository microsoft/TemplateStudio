using System;

using InkPoc.ViewModels;

using Windows.UI.Xaml.Controls;

namespace InkPoc.Views
{
    public sealed partial class SmartCanvasWithControlPage : Page
    {
        public SmartCanvasWithControlViewModel ViewModel { get; } = new SmartCanvasWithControlViewModel();

        public SmartCanvasWithControlPage()
        {
            InitializeComponent();
        }
    }
}
