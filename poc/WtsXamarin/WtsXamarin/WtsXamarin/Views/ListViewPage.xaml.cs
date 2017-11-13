using WtsXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListViewPage : ContentPage
	{
		public ListViewPage()
		{
			InitializeComponent();
            BindingContext = new ListViewViewModel();
        }
    }
}