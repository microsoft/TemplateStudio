// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using Microsoft.Templates.UI.Controls;

namespace Microsoft.Templates.UI.TemplateSelectors
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Information { get; set; }

        public DataTemplate Warning { get; set; }

        public DataTemplate Error { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Notification notification)
            {
                switch (notification.Type)
                {
                    case NotificationType.Warning:
                        return Warning;
                    case NotificationType.Error:
                        return Error;
                    default:
                        return Information;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
