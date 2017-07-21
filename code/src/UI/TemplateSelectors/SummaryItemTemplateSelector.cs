// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using Microsoft.Templates.UI.ViewModels.NewProject;

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
