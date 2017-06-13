using Microsoft.Templates.UI.ViewModels.NewItem;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class CodeLineTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultLineTemplate { get; set; }
        public DataTemplate NewLineTemplate { get; set; }
        public DataTemplate DeletedLineTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var line = item as CodeLineViewModel;
            if (line != null)
            {
                switch (line.Status)
                {
                    case LineStatus.Default:
                        return DefaultLineTemplate;
                    case LineStatus.New:
                        return NewLineTemplate;
                    case LineStatus.Deleted:
                        return DeletedLineTemplate;                        
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
