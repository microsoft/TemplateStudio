using Microsoft.Xaml.Interactivity;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WTSPrism.Behaviors
{
    public class PivotNavigationBehavior : Behavior<Pivot>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PivotItemUnloading += AssociatedObject_PivotItemUnloading;
            this.AssociatedObject.PivotItemLoading += AssociatedObject_PivotItemLoading;
        }

        private void AssociatedObject_PivotItemLoading(Pivot sender, PivotItemEventArgs args)
        {
            var navAwarePivot = ((args.Item.Content as Frame).Content as FrameworkElement).DataContext as INavigationAware;
            navAwarePivot?.OnNavigatedTo(null, null);
        }

        private void AssociatedObject_PivotItemUnloading(Pivot sender, PivotItemEventArgs args)
        {
            var navAwarePivot = ((args.Item.Content as Frame).Content as FrameworkElement).DataContext as INavigationAware;
            navAwarePivot?.OnNavigatingFrom(null, null,false);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PivotItemUnloading -= AssociatedObject_PivotItemUnloading;
            this.AssociatedObject.PivotItemLoading -= AssociatedObject_PivotItemLoading;
        }
    }
}
