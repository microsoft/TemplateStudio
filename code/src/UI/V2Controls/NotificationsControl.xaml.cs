// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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

        public async Task AddOrUpdateNotificationAsync(Notification notification)
        {
            bool isCurrent = false;
            if (notification != null)
            {
                Notification current = null;
                if (Notification?.Category == notification.Category)
                {
                    current = Notification;
                    isCurrent = true;
                }

                if (current == null)
                {
                    current = _notifications.FirstOrDefault(n => n.Category == notification.Category);
                }

                if (current != null)
                {
                    current.Update(notification);
                    if (isCurrent && notification.TimerType != TimerType.None)
                    {
                        current.ResetTimer(notification.TimerType);
                    }
                }
                else
                {
                    await AddNotificationAsync(notification);
                }
            }
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            if (notification != null)
            {
                if (Notification != null && Notification.Equals(notification))
                {
                    return;
                }

                RemoveCategoryNotifications(notification.Category);
                RemoveErrorCategoryNotifications(notification.ErrorCategory);
                RemoveCategoriesToOverride(notification.CategoriesToOverride);

                _notifications.Insert(0, notification);

                if (Notification == null || Notification.IsLowerOrEqualPriority(notification))
                {
                    await ShowNotificationAsync(notification);
                }
            }
        }

        private void RemoveCategoriesToOverride(IEnumerable<Category> categoriesToOverride)
        {
            if (categoriesToOverride != null)
            {
                foreach (var category in categoriesToOverride)
                {
                    RemoveCategoryNotifications(category);
                }
            }
        }

        public async Task CleanErrorNotificationsAsync(ErrorCategory replacementCategory)
        {
            RemoveErrorCategoryNotifications(replacementCategory);
            if (Notification?.ErrorCategory == replacementCategory)
            {
                await CloseAsync();
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

        private void RemoveCategoryNotifications(Category category)
        {
            if (category != Category.None)
            {
                PauseCategoryNotifications(category);
                _notifications.RemoveAll(n => n.Category == category);
            }
        }

        private void RemoveErrorCategoryNotifications(ErrorCategory errorCategory)
        {
            if (errorCategory != ErrorCategory.None)
            {
                _notifications.RemoveAll(n => n.ErrorCategory == errorCategory);
            }
        }

        private void PauseCategoryNotifications(Category category)
        {
            foreach (var notification in _notifications.Where(n => n.Category == category))
            {
                notification.StopCloseTimer();
            }
        }
    }
}
