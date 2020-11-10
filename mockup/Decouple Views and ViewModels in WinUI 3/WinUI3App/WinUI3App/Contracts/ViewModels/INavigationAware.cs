using Microsoft.UI.Xaml.Navigation;

namespace WinUI3App.Contracts.ViewModels
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object parameter);        

        void OnNavigatedFrom();

        void OnNavigatingFrom(NavigatingCancelEventArgs args);
    }
}
