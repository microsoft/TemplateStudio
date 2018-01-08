using XamarinUwpNative.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUwpNative.Views
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