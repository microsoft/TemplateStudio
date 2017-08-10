// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.ViewModels.NewItem;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class FileStatusTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NewFileTemplate { get; set; }
        public DataTemplate ModifiedFileTemplate { get; set; }
        public DataTemplate ConflictingFileTemplate { get; set; }
        public DataTemplate WarningFileTemplate { get; set; }
        public DataTemplate UnchangedFileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is BaseFileViewModel newItemFile)
            {
                switch (newItemFile.FileStatus)
                {
                    case FileStatus.NewFile:
                        return NewFileTemplate;
                    case FileStatus.ModifiedFile:
                        return ModifiedFileTemplate;
                    case FileStatus.ConflictingFile:
                        return ConflictingFileTemplate;
                    case FileStatus.WarningFile:
                        return WarningFileTemplate;
                    case FileStatus.Unchanged:
                        return UnchangedFileTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
