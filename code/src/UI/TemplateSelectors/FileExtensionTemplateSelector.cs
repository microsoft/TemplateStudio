using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class FileExtensionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CodeFileTemplate { get; set; }
        public DataTemplate ImageFileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var file = item as BaseFileViewModel;
            if (file != null)
            {
                switch (file.FileExtension)
                {
                    case FileExtension.Jpeg:
                    case FileExtension.Jpg:
                    case FileExtension.Png:
                        return ImageFileTemplate;
                    default:
                        return CodeFileTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
