using Caliburn.Micro;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using wts.ItemName.Services;
using wts.ItemName.ViewModels;

namespace wts.ItemName.Views
{
    public sealed partial class ShellPage : IShellView
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellPage()
        {
            InitializeComponent();
        }

        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            return container.RegisterNavigationService(shellFrame);
        }

        private void OnStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            ViewModel.StateChanged(e);
        }
    }
}
