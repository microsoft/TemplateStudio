using WtsXPlat.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXPlat.Mobile.Views
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
