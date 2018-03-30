using WtsXPlat.Core.Models;
using WtsXPlat.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXPlat.Mobile.Views
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
