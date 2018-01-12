using System;

using AzureMobileAuthPoc.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AzureMobileAuthPoc.Views
{
    public sealed partial class Page2Page : Page
    {
        public Page2ViewModel ViewModel { get; } = new Page2ViewModel();

        public Page2Page()
        {
            InitializeComponent();
        }
    }
}
