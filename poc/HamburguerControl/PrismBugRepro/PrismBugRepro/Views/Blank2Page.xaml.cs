using System;

using PrismBugRepro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PrismBugRepro.Views
{
    public sealed partial class Blank2Page : Page
    {
        private Blank2ViewModel ViewModel => DataContext as Blank2ViewModel;

        public Blank2Page()
        {
            InitializeComponent();
        }
    }
}
