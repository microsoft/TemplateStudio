using Wts.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wts.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BlankPage : ContentPage
    {
        public BlankPage()
        {
            InitializeComponent();
            BindingContext = new BlankViewModel();
        }
    }
}
