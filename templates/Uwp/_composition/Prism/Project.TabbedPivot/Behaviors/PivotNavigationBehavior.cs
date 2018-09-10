using Microsoft.Xaml.Interactivity;
using Prism.Windows.Navigation;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_ItemNamespace.Helpers;

namespace Param_ItemNamespace.Behaviors
{
    public class PivotNavigationBehavior : Behavior<Pivot>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var removedItem = e.RemovedItems.Cast<PivotItem>()
                .Select(i => i.GetPage<INavigationAware>()).FirstOrDefault();

            var addedItem = e.AddedItems.Cast<PivotItem>()
                .Select(i => i.GetPage<INavigationAware>()).FirstOrDefault();

            removedItem?.OnNavigatingFrom(null, null, false);
            addedItem?.OnNavigatedTo(null, null);
        }
    }
}
