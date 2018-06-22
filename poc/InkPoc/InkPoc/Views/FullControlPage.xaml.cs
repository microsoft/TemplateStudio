using System;

using InkPoc.ViewModels;

using Windows.UI.Xaml.Controls;

namespace InkPoc.Views
{
    public sealed partial class FullControlPage : Page
    {
        public FullControlViewModel ViewModel { get; } = new FullControlViewModel();

        public FullControlPage()
        {
            InitializeComponent();
        }
    }
}
