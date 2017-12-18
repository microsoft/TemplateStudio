using System;

using Windows.UI.Xaml.Controls;

using WTSGeneratedNavigation.ViewModels;

namespace WTSGeneratedNavigation.Views
{
    public sealed partial class TabbedPage : Page
    {
        private TabbedViewModel ViewModel => DataContext as TabbedViewModel;

        public TabbedPage()
        {
            InitializeComponent();
        }
    }
}
