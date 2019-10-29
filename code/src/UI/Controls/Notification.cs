// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Templates.UI.Mvvm;

namespace Microsoft.Templates.UI.Controls
{
    public enum NotificationType
    {
        Information = 0,
        Warning = 1,
        Error = 2,
    }

    public enum TimerType
    {
        None,
        Short,
        Large,
    }

    public enum ErrorCategory
    {
        None,
        NamingValidation,
    }

    public enum Category
    {
        None,
        TemplatesSync,
        TemplatesSyncError,
        AddTemplateValidation,
        RemoveTemplateValidation,
        RightClickItemHasNoChanges,
        ViewSufixValidation,
    }

    public class Notification : Observable
    {
        private bool _canClose;
        private string _message;
        private ICommand _closeCommand;
        private DispatcherTimer _closeTimer;

        public NotificationType Type { get; private set; }

        public ErrorCategory ErrorCategory { get; private set; }

        public Category Category { get; private set; }

        public TimerType TimerType { get; private set; }

        public string Message
        {
            get => _message;
            private set => SetProperty(ref _message, value);
        }

        public bool CanClose
        {
            get => _canClose;
            private set => SetProperty(ref _canClose, value);
        }

        // Only for Errors.
        // Errors could remove warning and notifications from pull it needed.
        // i.e. NamingValidation error can remove a RemoveTemplateValidation warning becaouse these errors are produced
        // by user but it there are some notification in pull not provided by user action is should keep in the pull (i.e. New templates available!)
        public IEnumerable<Category> CategoriesToOverride { get; private set; }

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new RelayCommand(OnClose));

        private Notification(TimerType timerType)
        {
            TimerType = timerType;
            double interval = GetInterval(timerType);

            _closeTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(interval) };
            _closeTimer.Tick += OnTick;
        }

        internal void UnsubscribeEventHandlers()
        {
            _closeTimer.Tick -= OnTick;
        }

        private static double GetInterval(TimerType timerType)
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

            return interval;
        }

        internal bool IsLowerOrEqualPriority(Notification notification)
        {
            return Type <= notification.Type;
        }

        public void Update(Notification notification)
        {
            Message = notification.Message;
            CanClose = notification.CanClose;

            if (notification.TimerType == TimerType.None)
            {
                StopCloseTimer();
            }
        }

        public void Remove()
        {
            _closeTimer.Interval = TimeSpan.FromSeconds(1);
            _closeTimer?.Start();
        }

        public static Notification Information(string message, Category category = Category.None, TimerType timerType = TimerType.Short, bool canClose = true)
        {
            return new Notification(timerType)
            {
                Type = NotificationType.Information,
                CanClose = canClose,
                Message = message,
                Category = category,
            };
        }

        public static Notification Warning(string message, Category category = Category.None, TimerType timerType = TimerType.Large, IEnumerable<Category> categoriesToOverride = null)
        {
            return new Notification(timerType)
            {
                Type = NotificationType.Warning,
                CanClose = true,
                Message = message,
                CategoriesToOverride = categoriesToOverride,
                Category = category,
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
                Category = Category.None,
            };
        }

        private async void OnClose()
        {
            await NotificationsControl.CloseAsync();
        }

        private async void OnTick(object sender, EventArgs e)
        {
            _closeTimer?.Stop();
            await NotificationsControl.CloseAsync();
        }

        public void StartCloseTimer()
        {
            if (TimerType != TimerType.None)
            {
                _closeTimer?.Start();
            }
        }

        public void StopCloseTimer() => _closeTimer?.Stop();

        public void ResetTimer(TimerType timerType)
        {
            _closeTimer.Stop();
            _closeTimer.Interval = TimeSpan.FromSeconds(GetInterval(timerType));
            _closeTimer.Start();
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
