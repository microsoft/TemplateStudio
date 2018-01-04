// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Templates.UI.Extensions;

namespace Microsoft.Templates.UI.V2Controls
{
    public partial class NotificationsControl : UserControl
    {
        public static NotificationsControl Instance { get; set; }

        public Notification Notification
        {
            get => (Notification)GetValue(NotificationProperty);
            set => SetValue(NotificationProperty, value);
        }

        public static readonly DependencyProperty NotificationProperty = DependencyProperty.Register("Notification", typeof(Notification), typeof(NotificationsControl), new PropertyMetadata(null));

        private readonly List<Notification> _notifications = new List<Notification>();

        public NotificationsControl()
        {
            Instance = this;
            InitializeComponent();
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            if (notification != null)
            {
                RemoveReplacementCategoryNotifications(notification);
                _notifications.Insert(0, notification);
                await ShowNotificationAsync(notification);
            }
        }

        public async Task CloseAsync()
        {
            Notification.StopCloseTimer();
            await fakeGrid.AnimateDoublePropertyAsync("Height", 0, 50, 500);
            _notifications.Remove(Notification);
            Notification = null;

            if (_notifications.Any())
            {
                await ShowNotificationAsync(_notifications.First());
            }
        }

        private async Task ShowNotificationAsync(Notification notification)
        {
            Notification?.StopCloseTimer();
            Notification = notification;
            await fakeGrid.AnimateDoublePropertyAsync("Height", 50, 0, 500);
            Notification.StartCloseTimer();
        }

        private void RemoveReplacementCategoryNotifications(Notification notification)
        {
            if (notification.ReplacementCategory != ReplacementCategory.None)
            {
                foreach (var notificationInPull in _notifications.Where(n => n.ReplacementCategory == notification.ReplacementCategory))
                {
                    notificationInPull.StopCloseTimer();
                }

                _notifications.RemoveAll(n => n.ReplacementCategory == notification.ReplacementCategory);
            }
        }
    }
}
