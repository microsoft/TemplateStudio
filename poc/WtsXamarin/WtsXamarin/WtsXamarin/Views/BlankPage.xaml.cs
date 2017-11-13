using WtsXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXamarin.Views
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