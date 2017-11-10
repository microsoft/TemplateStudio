using DragAndDropExample.Models;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DragAndDropExample.Services
{
    public class DragDropService
    {
        private static DependencyProperty ConfigurationProperty = DependencyProperty.RegisterAttached(
        "Configuration",
        typeof(DropConfiguration),
        typeof(DragDropService),
        new PropertyMetadata(null, OnConfigurationPropertyChanged));

        private static DependencyProperty VisualConfigurationProperty = DependencyProperty.RegisterAttached(
        "VisualConfiguration",
        typeof(VisualDropConfiguration),
        typeof(DragDropService),
        new PropertyMetadata(null, OnVisualConfigurationPropertyChanged));

        public static void SetConfiguration(DependencyObject dependencyObject, DropConfiguration value)
        {
            if (dependencyObject != null)
            {
                dependencyObject.SetValue(ConfigurationProperty, value);
            }
        }

        public static DropConfiguration GetConfiguration(DependencyObject dependencyObject)
        {
            return (DropConfiguration)dependencyObject.GetValue(ConfigurationProperty);
        }

        public static void SetVisualConfiguration(DependencyObject dependencyObject, VisualDropConfiguration value)
        {
            if (dependencyObject != null)
            {
                dependencyObject.SetValue(VisualConfigurationProperty, value);
            }
        }

        public static VisualDropConfiguration GetVisualConfiguration(DependencyObject dependencyObject)
        {
            return (VisualDropConfiguration)dependencyObject.GetValue(VisualConfigurationProperty);
        }

        private static void OnConfigurationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as UIElement;
            var configuration = GetConfiguration(element);
            ConfigureUIElement(element, configuration);

            var listview = element as ListViewBase;
            var listviewconfig = configuration as ListViewDropConfiguration;
            if(listview != null && listviewconfig != null)
            {
                ConfigureListView(listview, listviewconfig);
            }
        }

        private static void ConfigureUIElement(UIElement element, DropConfiguration configuration)
        {
            element.DragStarting += (sender, args) =>
            {
            };

            element.DragEnter += (sender, args) =>
            {
            };

            element.DragOver += (sender, args) =>
            {
                //TO-DO : Accepted operation needs to be calculated
                args.AcceptedOperation = configuration.AcceptedOperation;

                //Patch to move text data
                if (args.DataView.Contains(StandardDataFormats.Text))
                {
                    args.AcceptedOperation = DataPackageOperation.Move;
                }
            };

            element.Drop += async (sender, args) =>
            {
                await configuration.ProcessComandsAsync(args.DataView);
            };
        }

        private static void ConfigureListView(ListViewBase listview, ListViewDropConfiguration configuration)
        {
            listview.DragItemsStarting += (sender, args) =>
            {
                var data = new DragDropStartingData { Data = args.Data, Items = args.Items };
                configuration.OnDragItemsStartingCommand?.Execute(data);
            };

            listview.DragItemsCompleted += (sender, args) =>
            {
                var data = new DragDropCompletedData {DropResult = args.DropResult, Items = args.Items };
                configuration.OnDragItemsCompletedCommand?.Execute(data);
            };
        }

        private static void OnVisualConfigurationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as UIElement;
            var configuration = GetVisualConfiguration(element);

            element.DragStarting += (sender, args) =>
            {
                if (configuration.DropOverImage != null)
                {
                    args.DragUI.SetContentFromBitmapImage(configuration.StartingDragImage as BitmapImage);
                }
            };

            element.DragOver += (sender, args) =>
            {
                args.DragUIOverride.Caption = configuration.Caption;
                args.DragUIOverride.IsCaptionVisible = configuration.IsCaptionVisible;
                args.DragUIOverride.IsContentVisible = configuration.IsContentVisible;
                args.DragUIOverride.IsGlyphVisible = configuration.IsGlyphVisible;

                if (configuration.DropOverImage != null)
                {
                    args.DragUIOverride.SetContentFromBitmapImage(configuration.DropOverImage as BitmapImage);
                }
            };
        }
    }
}
