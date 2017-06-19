using Microsoft.Templates.UI.ViewModels.NewItem;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class NewItemFileTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AddedFileTemplate { get; set; }
        public DataTemplate ModifiedFileTemplate { get; set; }
        public DataTemplate ConflictingFileTemplate { get; set; }
        public DataTemplate WarningFileTemplate { get; set; }
        public DataTemplate UnchangedFileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var newItemFile = item as BaseNewItemFileViewModel;
            if (newItemFile != null)
            {
                switch (newItemFile.FileType)
                {
                    case FileType.AddedFile:
                        return AddedFileTemplate;
                    case FileType.ModifiedFile:
                        return ModifiedFileTemplate;
                    case FileType.ConflictingFile:
                        return ConflictingFileTemplate;
                    case FileType.WarningFile:
                        return WarningFileTemplate;
                    case FileType.Unchanged:
                        return UnchangedFileTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
