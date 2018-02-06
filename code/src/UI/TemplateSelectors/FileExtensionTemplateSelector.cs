// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class FileExtensionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CodeFileTemplate { get; set; }

        public DataTemplate ImageFileTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is NewItemFileViewModel file)
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
