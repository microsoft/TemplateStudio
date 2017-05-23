using Microsoft.Templates.Core;
using Microsoft.Templates.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class SummaryItemOptionsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PageOptionsTemplate { get; set; }
        public DataTemplate FeatureOptionsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var summaryItem = item as SavedTemplateViewModel;
            if (summaryItem != null)
            {
                if (summaryItem.TemplateType == TemplateType.Page)
                {
                    return PageOptionsTemplate;
                }
                else if (summaryItem.TemplateType == TemplateType.Feature)
                {
                    return FeatureOptionsTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
