using Microsoft.Xaml.Interactivity;
using Prism.Windows.Navigation;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
                .Select(i => GetPivotPage(i)).FirstOrDefault();

            var addedItem = e.AddedItems.Cast<PivotItem>()
                .Select(i => GetPivotPage(i)).FirstOrDefault();

            removedItem?.OnNavigatingFrom(null, null, false);
            addedItem?.OnNavigatedTo(null, null);
        }

        private static INavigationAware GetPivotPage(PivotItem pivotItem)
        {
            if (pivotItem.Content is Frame frame)
            {
                if (frame.Content is FrameworkElement frameworkElement)
                {
                    if (frameworkElement.DataContext is INavigationAware navigationAware)
                    {
                        return navigationAware;
                    }
                }
            }

            return null;
        }
    }
}
