using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;

namespace SplitViewTemplate.ViewModel
{
    public abstract class ViewModel : ViewModelBase
    {
        private Services.INavigationService _navigationService;
        public ViewModel(Services.INavigationService navigationService)
        {
            this._navigationService = navigationService;
        }

        public void SetNavigationFrame(Frame frame) => _navigationService.SetNavigationFrame(frame);
        public bool Navigate(string pageKey) => _navigationService.Navigate(pageKey);
    }
}
