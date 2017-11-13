using WtsXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CameraPage : ContentPage
	{
		public CameraPage ()
		{
			InitializeComponent ();
            BindingContext = new CameraViewModel();
		}
	}
}