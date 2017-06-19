using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.ViewModels;

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
