using MixedNavigationSample.CodeBehind.Services;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;

namespace MixedNavigationSample.CodeBehind.Views
{
    public sealed partial class StartPage : Page, INotifyPropertyChanged
    {
        public StartPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void StartButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Navigating to a ShellPage, this will replaces NavigationService frame for an inner frame to change navigation handling.
            NavigationService.Navigate<Views.ShellPage>();

            //Navigating now to a HomePage, this will be the first navigation on a NavigationPane menu
            NavigationService.Navigate<Views.HomePage>();
        }
    }
}
