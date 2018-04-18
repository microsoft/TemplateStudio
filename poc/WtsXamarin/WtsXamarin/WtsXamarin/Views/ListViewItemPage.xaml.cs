using WtsXamarin.Models;
using WtsXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXamarin.Views
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
