using Wts.Core.Helpers;

namespace Wts.Mobile.ViewModels
{
    public class MainViewModel : Observable
    {
        public MainViewModel()
        {
        }

        public string MainText { get; } = "Welcome to Xamarin.Forms from Windows Template Studio!";
    }
}
