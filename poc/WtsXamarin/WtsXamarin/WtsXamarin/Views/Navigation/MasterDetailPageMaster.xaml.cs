using WtsXamarin.ViewModels.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXamarin.Views.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageMaster : ContentPage
    {
        public ListView PrimaryListView;

        public MasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new MasterDetailPageMasterViewModel();
            PrimaryListView = PrimaryMenuItemsListView;
        }
    }
}