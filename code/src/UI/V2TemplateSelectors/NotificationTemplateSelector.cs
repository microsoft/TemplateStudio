// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using Microsoft.Templates.UI.V2Controls;

namespace Microsoft.Templates.UI.V2TemplateSelectors
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Information { get; set; }

        public DataTemplate Warning { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var notification = item as Notification;
            if (notification != null)
            {
                switch (notification.Type)
                {
                    case NotificationType.Warning:
                        return Warning;
                    default:
                        return Information;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
