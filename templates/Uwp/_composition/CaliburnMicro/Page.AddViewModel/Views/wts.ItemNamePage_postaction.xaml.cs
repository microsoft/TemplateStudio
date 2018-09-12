//{[{
using Param_ItemNamespace.ViewModels;
//}]}
namespace Param_ItemNamespace.Views
{
        public wts.ItemNamePage()
        {
            InitializeComponent();
        }

        //{[{
        private wts.ItemNameViewModel ViewModel
        {
            get { return ViewModelLocator.Current.wts.ItemNameViewModel; }
        }
        //}]}
}
