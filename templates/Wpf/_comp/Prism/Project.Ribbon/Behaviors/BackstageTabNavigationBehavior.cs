using System.Windows.Controls;

using Fluent;

using Microsoft.Xaml.Behaviors;
using Prism.Regions;

namespace Param_RootNamespace.Behaviors
{
    public class BackstageTabNavigationBehavior : Behavior<BackstageTabControl>
    {
        private IRegionManager _regionManager;

        public void Initialize(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

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

        private void OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is BackstageTabItem tabItem)
            {
                var viewName = tabItem.Tag as string;
                if (tabItem.Content == null)
                {
                    var contentControl = new ContentControl();
                    tabItem.Content = contentControl;
                    RegionManager.SetRegionName(contentControl, viewName);
                    RegionManager.SetRegionManager(contentControl, _regionManager);
                }

                _regionManager.RequestNavigate(viewName, viewName);
            }
        }
    }
}