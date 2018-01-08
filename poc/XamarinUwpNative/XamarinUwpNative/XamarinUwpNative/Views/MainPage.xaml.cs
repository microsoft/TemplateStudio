using XamarinUwpNative.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUwpNative.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
            BindingContext = new MainViewModel();
        }
    }
}