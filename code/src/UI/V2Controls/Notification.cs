// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.V2Controls
{
    public enum NotificationType
    {
        Information,
        Warning,
        Error
    }

    public enum TimerType
    {
        None,
        Short,
        Large
    }

    public enum ErrorCategory
    {
        None,
        NamingValidation
    }

    public enum Category
    {
        None,
        TemplatesSync,
        RemoveTemplateValidation,
        RightClickItemHasNoChanges
    }

    public class Notification
    {
        private ICommand _closeCommand;
        private DispatcherTimer _closeTimer;

        public NotificationType Type { get; private set; }

        public ErrorCategory ErrorCategory { get; private set; }

        public Category Category { get; private set; }

        public string Message { get; private set; }

        // Only for Errors.
        // Errors could remove warning and notifications from pull it needed.
        // i.e. NamingValidation error can remove a RemoveTemplateValidation warning becaouse these errors are produced
        // by user but it there are some notification in pull not provided by user action is should keep in the pull (i.e. New templates available!)
        public IEnumerable<Category> CategoriesToOverride { get; private set; }

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));

        private Notification(TimerType timerType)
        {
            if (timerType != TimerType.None)
            {
                double interval = 0.0;
                switch (timerType)
                {
                    case TimerType.Short:
                        interval = 3;
                        break;
                    case TimerType.Large:
                        interval = 6;
                        break;
                }

                _closeTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(interval) };
                _closeTimer.Tick += OnTick;
            }
        }

        public static Notification Information(string message, Category category = Category.None, TimerType timerType = TimerType.Short)
        {
            return new Notification(timerType)
            {
                Type = NotificationType.Information,
                Message = message,
                Category = category
            };
        }

        public static Notification Warning(string message, Category category = Category.None, TimerType timerType = TimerType.Large)
        {
            return new Notification(timerType)
            {
                Type = NotificationType.Warning,
                Message = message,
                Category = category
            };
        }

        public static Notification Error(string message, ErrorCategory errorCategory = ErrorCategory.None, IEnumerable<Category> categoriesToOverride = null)
        {
            return new Notification(TimerType.None)
            {
                Type = NotificationType.Error,
                Message = message,
                ErrorCategory = errorCategory,
                CategoriesToOverride = categoriesToOverride,
                Category = Category.None
            };
        }

        private async void OnClose()
        {
            await NotificationsControl.Instance.CloseAsync();
        }

        private async void OnTick(object sender, EventArgs e)
        {
            _closeTimer?.Stop();
            await NotificationsControl.Instance.CloseAsync();
        }

        public void StartCloseTimer()
        {
            _closeTimer?.Start();
        }

        public void StopCloseTimer()
        {
            _closeTimer?.Stop();
        }

        public override bool Equals(object obj)
        {
            var result = false;
            if (obj is Notification notification)
            {
                result = notification.Category == Category && notification.Message == Message;
            }

            return result;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
