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
    public class SummaryItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MicrosoftTemplate { get; set; }
        public DataTemplate CommunityTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var summaryItem = item as SavedTemplateViewModel;
            if (summaryItem != null)
            {
                if (summaryItem.Author.ToLower() == "microsoft")
                {
                    return MicrosoftTemplate;
                }
            }
            return CommunityTemplate;
        }
    }
}
