using XamarinUwpNative.Models;
using XamarinUwpNative.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinUwpNative.Views
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
