//{[{
using System.Linq;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Extensions;
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
        }

        //{[{
        private void wts.ItemNamePage_Loaded(object sender, RoutedEventArgs e)
        {
            var pivotPage = this.FindParent<Pivot>();
            if (pivotPage != null)
            {
                pivotPage.SelectionChanged += PivotPage_SelectionChanged;
            }
        }

        private void PivotPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: Start or stop video
        }
        //}]}
    }
}