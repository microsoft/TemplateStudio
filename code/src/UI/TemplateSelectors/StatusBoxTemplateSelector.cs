// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;

using Microsoft.Templates.UI.Controls;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class StatusBoxTemplateSelector : DataTemplateSelector
    {
        public DataTemplate EmptyStatusTemplate { get; set; }
        public DataTemplate InformationStatusTemplate { get; set; }
        public DataTemplate WarningStatusTemplate { get; set; }
        public DataTemplate ErrorStatusTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var status = item as StatusViewModel;
            if (status != null)
            {
                switch (status.Status)
                {
                    case StatusType.Empty:
                        return EmptyStatusTemplate;
                    case StatusType.Information:
                        return InformationStatusTemplate;
                    case StatusType.Warning:
                        return WarningStatusTemplate;
                    case StatusType.Error:
                        return ErrorStatusTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
