// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.Core;
using Microsoft.Templates.UI.ViewModels.NewProject;

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
