//{[{
using Param_RootNamespace.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : /*{[{*/IBackNavigationHandler/*}]}*/
    {
//^^
//{[{
        public event EventHandler<bool> OnPageCanGoBackChanged;
//}]}

        public SampleOrder Selected
        {
        }

        private void OnItemClick()
        {
            if (_twoPaneView.Mode == WinUI.TwoPaneViewMode.SinglePane)
            {
//^^
//{[{
                OnPageCanGoBackChanged?.Invoke(this, true);
//}]}
                TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2;
            }
        }

        private void OnModeChanged(WinUI.TwoPaneView twoPaneView)
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
    }
}