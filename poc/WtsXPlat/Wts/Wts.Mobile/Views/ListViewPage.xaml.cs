using Wts.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Wts.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewPage : ContentPage
    {

        public ListViewPage()
        {
            InitializeComponent();
            BindingContext = new ListViewViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as ListViewViewModel;
            await vm.LoadDataAsync();
            vm.SelectedItem = vm.SampleData[0];

        }
    }
}
