using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Param_RootNamespace.Models;

namespace Param_RootNamespace.TemplateSelectors
{
    public class SharedContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }

        public DataTemplate StorageItemsTemplate { get; set; }

        public DataTemplate WebLinkTemplate { get; set; }

        public SharedContentTemplateSelector()
        {
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var sharedData = item as SharedDataModelBase;
            if (sharedData != null)
            {
                if (sharedData.DataFormat == StandardDataFormats.WebLink)
                {
                    return WebLinkTemplate;
                }
                else if (sharedData.DataFormat == StandardDataFormats.StorageItems)
                {
                    return StorageItemsTemplate;
                }
            }

            return DefaultTemplate;
        }
    }
}
