using Wts.Core.Models;
using Wts.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wts.Mobile.Views
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
