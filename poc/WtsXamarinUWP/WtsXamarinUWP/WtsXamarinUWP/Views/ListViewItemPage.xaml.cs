using WtsXamarinUWP.Core.Models;
using WtsXamarinUWP.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXamarinUWP.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewItemPage : ContentPage
    {
        public ListViewItemPage(object parameter)
        {
            InitializeComponent();
            BindingContext = new ListViewItemViewModel(parameter as SampleOrder);
        }
    }
}
