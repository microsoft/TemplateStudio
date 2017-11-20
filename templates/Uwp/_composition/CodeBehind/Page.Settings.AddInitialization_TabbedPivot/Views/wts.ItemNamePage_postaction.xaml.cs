//{[{
using Windows.UI.Xaml;
//}]}
namespace Param_ItemNamespace.Views
{
    public sealed partial class wts.ItemNamePage : Page, INotifyPropertyChanged
    {
        public wts.ItemNamePage()
        {
            //{[{
            Loaded += wts.ItemNamePage_Loaded;
            //}]}
            InitializeComponent();
        }

        //{[{
        private void wts.ItemNamePage_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }
        //}]}
    }
}
