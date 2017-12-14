using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using WTSGeneratedPivot.ViewModels;

namespace WTSGeneratedPivot.Views
{
    public sealed partial class PivotPage : Page
    {
        private PivotViewModel ViewModel => DataContext as PivotViewModel;

        public PivotPage()
        {
            // We use NavigationCacheMode.Required to keep track the selected item on navigation. For further information see the following links.
            // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/windows.ui.xaml.controls.page.navigationcachemode.aspx
            // https://msdn.microsoft.com/en-us/library/windows/apps/xaml/Hh771188.aspx
            NavigationCacheMode = NavigationCacheMode.Required;
            InitializeComponent();
        }
    }
}
