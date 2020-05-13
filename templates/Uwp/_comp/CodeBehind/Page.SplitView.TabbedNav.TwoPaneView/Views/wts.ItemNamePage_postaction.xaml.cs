//{[{
using Param_RootNamespace.Helpers;
//}]}
namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNamePage : /*{[{*/IBackNavigationHandler/*}]}*/
    {
//^^
//{[{
        public event EventHandler<bool> OnPageCanGoBackChanged;
//}]}

        public SampleOrder Selected
        {
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
//^^
//{[{
                OnPageCanGoBackChanged?.Invoke(this, true);
//}]}
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2;
            }
        }

        private void OnModeChanged(WinUI.TwoPaneView sender, object args)
        {
            if (twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
//^^
//{[{
                OnPageCanGoBackChanged?.Invoke(this, true);
//}]}
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2;
            }
            else
            {
//^^
//{[{
                OnPageCanGoBackChanged?.Invoke(this, false);
//}]}
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1;
            }
        }
//^^
//{[{
        public void GoBack()
        {
            if (TwoPanePriority == WinUI.TwoPaneViewPriority.Pane2)
            {
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane1;
                OnPageCanGoBackChanged?.Invoke(this, false);
            }
        }
//}]}
        public event PropertyChangedEventHandler PropertyChanged;
    }
}