using System;

using PrismBugRepro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PrismBugRepro.Views
{
    public sealed partial class Blank1Page : Page
    {
        private Blank1ViewModel ViewModel => DataContext as Blank1ViewModel;

        public Blank1Page()
        {
            InitializeComponent();
        }
    }
}
