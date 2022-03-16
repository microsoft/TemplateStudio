using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Fluent;
using Microsoft.Xaml.Behaviors;
using Param_RootNamespace.Contracts.Views;

namespace Param_RootNamespace.Behaviors
{
    public class BackstageTabNavigationBehavior : Behavior<BackstageTabControl>
    {
        private IServiceProvider _serviceProvider;

        public BackstageTabNavigationBehavior()
        {
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                var frame = new Frame()
                {
                    Focusable = false,
                    NavigationUIVisibility = NavigationUIVisibility.Hidden
                };
                frame.Navigated += OnNavigated;
                tabItem.Content = frame;
                var pageType = Type.GetType(tabItem.Tag as string);
                var page = _serviceProvider.GetService(pageType) as Page;
                frame.Navigate(page);
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.ExtraData);
            }
        }
    }
}
