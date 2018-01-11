using System;

using PrismBugRepro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PrismBugRepro.Views
{
    public sealed partial class Blank3Page : Page
    {
        private Blank3ViewModel ViewModel => DataContext as Blank3ViewModel;

        public Blank3Page()
        {
            InitializeComponent();
        }
    }
}
