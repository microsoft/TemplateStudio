using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

using Param_RootNamespace.Models;

namespace Param_RootNamespace.Services.DragAndDrop
{
    // For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/features/drag-and-drop.md
    public class DragDropService
    {
        private static DependencyProperty configurationProperty = DependencyProperty.RegisterAttached(
        "Configuration",
        typeof(DropConfiguration),
        typeof(DragDropService),
        new PropertyMetadata(null, OnConfigurationPropertyChanged));

        private static DependencyProperty visualConfigurationProperty = DependencyProperty.RegisterAttached(
        "VisualConfiguration",
        typeof(VisualDropConfiguration),
        typeof(DragDropService),
        new PropertyMetadata(null, OnVisualConfigurationPropertyChanged));

        public static void SetConfiguration(DependencyObject dependencyObject, DropConfiguration value)
        {
            if (dependencyObject != null)
            {
                dependencyObject.SetValue(configurationProperty, value);
            }
        }

        public static DropConfiguration GetConfiguration(DependencyObject dependencyObject)
        {
            return (DropConfiguration)dependencyObject.GetValue(configurationProperty);
        }

        public static void SetVisualConfiguration(DependencyObject dependencyObject, VisualDropConfiguration value)
        {
            if (dependencyObject != null)
            {
                dependencyObject.SetValue(visualConfigurationProperty, value);
            }
        }

        public static VisualDropConfiguration GetVisualConfiguration(DependencyObject dependencyObject)
        {
            return (VisualDropConfiguration)dependencyObject.GetValue(visualConfigurationProperty);
        }

        private static void OnConfigurationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as UIElement;
            var configuration = GetConfiguration(element);
            ConfigureUIElement(element, configuration);

            var listview = element as ListViewBase;
            var listviewconfig = configuration as ListViewDropConfiguration;
            if (listview != null && listviewconfig != null)
            {
                ConfigureListView(listview, listviewconfig);
            }
        }

        private static void ConfigureUIElement(UIElement element, DropConfiguration configuration)
        {
        }

        private static void ConfigureListView(ListViewBase listview, ListViewDropConfiguration configuration)
        {
        }

        private static void OnVisualConfigurationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as UIElement;
            var configuration = GetVisualConfiguration(element);

            element.DragStarting += (sender, args) =>
            {
                if (configuration.DropOverImage != null)
                {
                    args.DragUI.SetContentFromBitmapImage(configuration.DragStartingImage as BitmapImage);
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
