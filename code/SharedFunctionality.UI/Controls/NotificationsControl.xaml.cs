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
using Microsoft.Templates.UI.Styles;

namespace Microsoft.Templates.UI.Controls
{
    public partial class NotificationsControl : UserControl
    {
        private static NotificationsControl _instance;

        public Notification Notification
        {
            get => (Notification)GetValue(NotificationProperty);
            set => SetValue(NotificationProperty, value);
        }

        public static readonly DependencyProperty NotificationProperty = DependencyProperty.Register("Notification", typeof(Notification), typeof(NotificationsControl), new PropertyMetadata(null));

        private readonly List<Notification> _notifications = new List<Notification>();

        public NotificationsControl()
        {
            Resources.MergedDictionaries.Add(AllStylesDictionary.GetMergeDictionary());

            _instance = this;
            InitializeComponent();
        }

        public static async Task AddOrUpdateNotificationAsync(Notification notification)
            => await SafeInvokeAsync(async ()
                => await _instance.InternalAddOrUpdateNotificationAsync(notification));

        public static void UnsubscribeEventHandlers()
            => SafeInvoke(()
                => _instance.InternalUnsubscribeEventHandlers());

        public static async Task AddNotificationAsync(Notification notification)
            => await SafeInvokeAsync(async ()
                => await _instance.InternalAddNotificationAsync(notification));

        public static void RemoveNotification()
            => SafeInvoke(()
                => _instance.InternalRemoveNotification());

        public static async Task CleanCategoryNotificationsAsync(Category category)
            => await SafeInvokeAsync(async ()
                => await _instance.InternalCleanCategoryNotificationsAsync(category));

        public static async Task CleanErrorNotificationsAsync(ErrorCategory replacementCategory)
            => await SafeInvokeAsync(async ()
                => await _instance.InternalCleanErrorNotificationsAsync(replacementCategory));

        public static async Task CloseAsync()
            => await SafeInvokeAsync(async ()
                => await _instance.InternalCloseAsync());

        private static async Task SafeInvokeAsync(Func<Task> func)
        {
            if (_instance != null)
            {
                await func();
            }
        }

        private static void SafeInvoke(Action func)
        {
            if (_instance != null)
            {
                func();
            }
        }

        private async Task InternalAddOrUpdateNotificationAsync(Notification notification)
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

        private void InternalUnsubscribeEventHandlers()
        {
            foreach (var notification in _notifications)
            {
                notification.UnsubscribeEventHandlers();
            }
        }

        private async Task InternalAddNotificationAsync(Notification notification)
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
                    await ShowNotificationAsync();
                }
            }
        }

        private void InternalRemoveNotification()
        {
            Notification?.Remove();
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

        private async Task InternalCleanErrorNotificationsAsync(ErrorCategory replacementCategory)
        {
            RemoveErrorCategoryNotifications(replacementCategory);
            if (Notification?.ErrorCategory == replacementCategory)
            {
                await InternalCloseAsync();
            }
        }

        private async Task InternalCleanCategoryNotificationsAsync(Category category)
        {
            RemoveCategoryNotifications(category);
            if (Notification?.Category == category)
            {
                await InternalCloseAsync();
            }
        }

        private async Task InternalCloseAsync()
        {
            try
            {
                Notification.StopCloseTimer();
                await fakeGrid.AnimateDoublePropertyAsync("Height", 0, 50, 500);
                _notifications.Remove(Notification);
                Notification = null;

                if (_notifications.Any())
                {
                    await ShowNotificationAsync();
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
            }
        }

        private async Task ShowNotificationAsync()
        {
            Notification?.StopCloseTimer();
            Notification = _notifications.First();
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
