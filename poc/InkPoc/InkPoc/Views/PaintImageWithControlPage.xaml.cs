using System;

using InkPoc.ViewModels;

using Windows.UI.Xaml.Controls;

namespace InkPoc.Views
{
    public sealed partial class PaintImageWithControlPage : Page
    {
        public PaintImageWithControlViewModel ViewModel { get; } = new PaintImageWithControlViewModel();

        public PaintImageWithControlPage()
        {
            InitializeComponent();
        }
    }
}
