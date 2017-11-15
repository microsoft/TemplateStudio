using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
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

        private static void OnConfigurationPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is UIElement element)
            {
                var configuration = GetConfiguration(element);

                element.DragStarting += (sender, args) =>
                {
                    if(configuration.StartingDragImage != null)
                    {
                        args.DragUI.SetContentFromBitmapImage(configuration.StartingDragImage as BitmapImage);
                    }
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

                    args.DragUIOverride.Caption = configuration.Caption;
                    args.DragUIOverride.IsCaptionVisible = configuration.IsCaptionVisible;
                    args.DragUIOverride.IsContentVisible = configuration.IsContentVisible;
                    args.DragUIOverride.IsGlyphVisible = configuration.IsGlyphVisible;
                    if (configuration.DropOverImage != null)
                    {
                        args.DragUIOverride.SetContentFromBitmapImage(configuration.DropOverImage as BitmapImage);
                    }
                };

                element.Drop += async (sender, args) =>
                {
                    await ProcessComandsAsync(configuration, args.DataView);
                    args.Handled = true;
                };
            }
        }

        private static async Task ProcessComandsAsync(DropConfiguration configuration, DataPackageView dataview)
        {
            if(configuration.OnDropDataViewCommand != null)
            {
                configuration.OnDropDataViewCommand.Execute(dataview);
            }


            if (dataview.Contains(StandardDataFormats.ApplicationLink) && configuration.OnDropApplicationLinkCommand != null)
            {
                var uri = await dataview.GetApplicationLinkAsync();
                configuration.OnDropApplicationLinkCommand.Execute(uri);
            }

            if (dataview.Contains(StandardDataFormats.Bitmap) && configuration.OnDropBitmapCommand != null)
            {
                var stream = await dataview.GetBitmapAsync();
                configuration.OnDropBitmapCommand.Execute(stream);
            }

            if (dataview.Contains(StandardDataFormats.Html) && configuration.OnDropHtmlCommand != null)
            {
                var html = await dataview.GetHtmlFormatAsync();
                configuration.OnDropHtmlCommand.Execute(html);
            }

            if (dataview.Contains(StandardDataFormats.Rtf) && configuration.OnDropRtfCommand != null)
            {
                var rtf = await dataview.GetRtfAsync();
                configuration.OnDropRtfCommand.Execute(rtf);
            }

            if (dataview.Contains(StandardDataFormats.StorageItems) && configuration.OnDropStorageItemsCommand != null)
            {
                var storageItems = await dataview.GetStorageItemsAsync();
                configuration.OnDropStorageItemsCommand.Execute(storageItems);
            }

            if (dataview.Contains(StandardDataFormats.Text) && configuration.OnDropTextCommand != null)
            {
                var text = await dataview.GetTextAsync();
                configuration.OnDropTextCommand.Execute(text);
            }

            if (dataview.Contains(StandardDataFormats.WebLink) && configuration.OnDropWebLinkCommand != null)
            {
                var uri = await dataview.GetWebLinkAsync();
                configuration.OnDropWebLinkCommand.Execute(uri);
            }
        }
    }
}
