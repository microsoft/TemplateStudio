//{[{
using Param_RootNamespace.ViewModels;
//}]}
namespace Param_RootNamespace.Views
{
        public wts.ItemNamePage()
        {
            InitializeComponent();
        }

        //{[{
        private wts.ItemNameViewModel ViewModel
        {
            get { return DataContext as wts.ItemNameViewModel; }
        }
        //}]}
}
