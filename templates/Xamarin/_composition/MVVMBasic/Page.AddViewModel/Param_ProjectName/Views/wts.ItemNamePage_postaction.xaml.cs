//{[{
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class wts.ItemNamePage : ContentPage
    {
        public wts.ItemNamePage ()
        {
            InitializeComponent ();
            //{[{
            BindingContext = new wts.ItemNameViewModel();
            //}]}
        }
    }
}