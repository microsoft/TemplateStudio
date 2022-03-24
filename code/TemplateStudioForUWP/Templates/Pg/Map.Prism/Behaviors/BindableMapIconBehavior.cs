using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace Param_RootNamespace.Behaviors
{
    public class BindableMapIconBehavior : Behavior<MapControl>
    {
        public IEnumerable<MapIcon> MapIcons
        {
            get { return (IEnumerable<MapIcon>)GetValue(MapIconsProperty); }
            set { SetValue(MapIconsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapIconsProperty =
            DependencyProperty.Register("MapIcons", typeof(IEnumerable<MapIcon>), typeof(BindableMapIconBehavior), new PropertyMetadata(null, (o, e) => ((BindableMapIconBehavior)o).MapIconCollectionChanged(e)));

        private void MapIconCollectionChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldCollection = e.OldValue as INotifyCollectionChanged;
            var newCollection = e.NewValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= OnMapIconsCollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += OnMapIconsCollectionChanged;
            }
        }

        private void OnMapIconsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (AssociatedObject == null)
            {
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var mapIcon in e.NewItems)
                {
                    AssociatedObject.MapElements.Add(mapIcon as MapIcon);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var mapIcon in e.OldItems)
                {
                    AssociatedObject.MapElements.Remove(mapIcon as MapIcon);
                }
            }
        }
    }
}
