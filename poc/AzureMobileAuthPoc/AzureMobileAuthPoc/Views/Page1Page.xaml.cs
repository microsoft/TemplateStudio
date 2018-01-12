using System;

using AzureMobileAuthPoc.ViewModels;

using Windows.UI.Xaml.Controls;

namespace AzureMobileAuthPoc.Views
{
    public sealed partial class Page1Page : Page
    {
        public Page1ViewModel ViewModel { get; } = new Page1ViewModel();

        public Page1Page()
        {
            InitializeComponent();
        }
    }
}
